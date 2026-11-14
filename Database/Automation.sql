/* =====================================================================================
   Workflow Automation Center - Schema
   -------------------------------------------------------------------------------------
   Nguyên tắc:
     - CHỈ THÊM: không DROP, không DELETE dữ liệu nghiệp vụ.
     - Idempotent: chạy lại nhiều lần không lỗi, không tạo trùng.
     - Chạy trên database phát triển:
         sqlcmd -S "localhost\SQLEXPRESS" -d "QuanLyLopTinHocVaNgoaiNguNganHan" -E -C -f 65001 -b -i Database\Automation.sql
   ===================================================================================== */

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* =====================================================================================
   0. KIỂM TRA HIỆN TRẠNG (chỉ đọc)
   ===================================================================================== */
PRINT N'--- 0. Hiện trạng ---';

SELECT
    CASE WHEN COL_LENGTH(N'dbo.LopHoc', N'TrangThai')    IS NULL THEN N'THIẾU' ELSE N'CÓ' END AS Cot_TrangThai,
    CASE WHEN COL_LENGTH(N'dbo.LopHoc', N'SiSoToiThieu') IS NULL THEN N'THIẾU' ELSE N'CÓ' END AS Cot_SiSoToiThieu;

SELECT name AS BangAutomationDaCo
FROM sys.tables
WHERE name IN (N'AutomationRule', N'AutomationCondition', N'AutomationAction',
               N'AutomationExecution', N'AutomationExecutionLog', N'AutomationTask', N'ThongBao')
ORDER BY name;
GO

/* =====================================================================================
   1. BỔ SUNG LopHoc: TrangThai + SiSoToiThieu
   ===================================================================================== */
PRINT N'--- 1. Bổ sung cột cho LopHoc ---';

IF COL_LENGTH(N'dbo.LopHoc', N'TrangThai') IS NULL
BEGIN
    ALTER TABLE dbo.LopHoc ADD
        TrangThai VARCHAR(20) NOT NULL
            CONSTRAINT DF_LopHoc_TrangThai DEFAULT ('SapKhaiGiang');
    PRINT N'  + Thêm cột LopHoc.TrangThai';
END
GO

IF COL_LENGTH(N'dbo.LopHoc', N'SiSoToiThieu') IS NULL
BEGIN
    ALTER TABLE dbo.LopHoc ADD
        SiSoToiThieu SMALLINT NOT NULL
            CONSTRAINT DF_LopHoc_SiSoToiThieu DEFAULT (0);
    PRINT N'  + Thêm cột LopHoc.SiSoToiThieu';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_LopHoc_TrangThai')
    ALTER TABLE dbo.LopHoc ADD CONSTRAINT CK_LopHoc_TrangThai
        CHECK (TrangThai IN ('SapKhaiGiang','DangHoc','DaKetThuc','Huy'));
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_LopHoc_SiSo')
    ALTER TABLE dbo.LopHoc ADD CONSTRAINT CK_LopHoc_SiSo
        CHECK (SiSoToiThieu >= 0 AND SiSoToiThieu <= SiSoToiDa);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.LopHoc') AND name = N'IX_LopHoc_TrangThai')
    CREATE INDEX IX_LopHoc_TrangThai ON dbo.LopHoc (TrangThai);
GO

/* =====================================================================================
   2. Bảng cấu hình Rule
   ===================================================================================== */
PRINT N'--- 2. Tạo bảng AutomationRule / AutomationCondition / AutomationAction ---';

IF OBJECT_ID(N'dbo.AutomationRule', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AutomationRule
    (
        RuleID          INT            IDENTITY(1,1) NOT NULL,
        RuleName        NVARCHAR(150)  NOT NULL,
        Description     NVARCHAR(500)  NULL,
        TriggerType     VARCHAR(40)    NOT NULL,
        Priority        INT            NOT NULL CONSTRAINT DF_AutomationRule_Priority DEFAULT (100),
        IsActive        BIT            NOT NULL CONSTRAINT DF_AutomationRule_IsActive DEFAULT (1),
        IsDeleted       BIT            NOT NULL CONSTRAINT DF_AutomationRule_IsDeleted DEFAULT (0),
        CooldownMinutes INT            NULL,
        CreatedAt       DATETIME2(0)   NOT NULL CONSTRAINT DF_AutomationRule_CreatedAt DEFAULT (SYSDATETIME()),
        CreatedBy       VARCHAR(50)    NULL,
        UpdatedAt       DATETIME2(0)   NULL,
        UpdatedBy       VARCHAR(50)    NULL,
        RowVersion      ROWVERSION     NOT NULL,

        CONSTRAINT PK_AutomationRule PRIMARY KEY CLUSTERED (RuleID),
        CONSTRAINT CK_AutomationRule_TriggerType CHECK (TriggerType IN
            ('CLASS_CREATED','CLASS_ENROLLMENT_CHANGED','CLASS_STATUS_CHANGED')),
        CONSTRAINT CK_AutomationRule_Priority CHECK (Priority BETWEEN 0 AND 10000),
        CONSTRAINT CK_AutomationRule_RuleName CHECK (LEN(LTRIM(RTRIM(RuleName))) > 0),
        CONSTRAINT CK_AutomationRule_Cooldown CHECK (CooldownMinutes IS NULL OR CooldownMinutes >= 0),
        CONSTRAINT FK_AutomationRule_CreatedBy FOREIGN KEY (CreatedBy)
            REFERENCES dbo.TaiKhoan (TenDangNhap)
    );

    PRINT N'  + AutomationRule';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationRule') AND name = N'UX_AutomationRule_RuleName')
    CREATE UNIQUE INDEX UX_AutomationRule_RuleName
        ON dbo.AutomationRule (RuleName) WHERE IsDeleted = 0;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationRule') AND name = N'IX_AutomationRule_Trigger')
    CREATE INDEX IX_AutomationRule_Trigger
        ON dbo.AutomationRule (TriggerType, IsActive, Priority) WHERE IsDeleted = 0;
GO

IF OBJECT_ID(N'dbo.AutomationCondition', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AutomationCondition
    (
        ConditionID INT           IDENTITY(1,1) NOT NULL,
        RuleID      INT           NOT NULL,
        FieldCode   VARCHAR(40)   NOT NULL,
        Operator    VARCHAR(30)   NOT NULL,
        ValueText   NVARCHAR(200) NULL,
        ValueNumber DECIMAL(18,2) NULL,
        SortOrder   INT           NOT NULL,

        CONSTRAINT PK_AutomationCondition PRIMARY KEY CLUSTERED (ConditionID),
        CONSTRAINT FK_AutomationCondition_Rule FOREIGN KEY (RuleID)
            REFERENCES dbo.AutomationRule (RuleID) ON DELETE CASCADE,
        CONSTRAINT UQ_AutomationCondition_RuleOrder UNIQUE (RuleID, SortOrder),
        CONSTRAINT CK_AutomationCondition_Field CHECK (FieldCode IN
            ('CurrentEnrollment','ClassCapacity','MinimumEnrollment',
             'HasTeacher','HasSchedule','DaysUntilStart','DaysUntilEnd')),
        CONSTRAINT CK_AutomationCondition_Operator CHECK (Operator IN
            ('EQUALS','NOT_EQUALS','GREATER_THAN','GREATER_THAN_OR_EQUAL',
             'LESS_THAN','LESS_THAN_OR_EQUAL','IS_TRUE','IS_FALSE')),
        CONSTRAINT CK_AutomationCondition_TypeMatch CHECK
        (
            (FieldCode IN ('CurrentEnrollment','ClassCapacity','MinimumEnrollment',
                           'DaysUntilStart','DaysUntilEnd')
             AND Operator IN ('EQUALS','NOT_EQUALS','GREATER_THAN',
                              'GREATER_THAN_OR_EQUAL','LESS_THAN','LESS_THAN_OR_EQUAL')
             AND ValueNumber IS NOT NULL AND ValueText IS NULL)
            OR
            (FieldCode IN ('HasTeacher','HasSchedule')
             AND Operator IN ('IS_TRUE','IS_FALSE')
             AND ValueNumber IS NULL AND ValueText IS NULL)
            OR
            (FieldCode IN ('HasTeacher','HasSchedule')
             AND Operator IN ('EQUALS','NOT_EQUALS')
             AND ValueNumber IS NULL AND ValueText IN (N'true', N'false'))
        )
    );

    PRINT N'  + AutomationCondition';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationCondition') AND name = N'IX_AutomationCondition_Rule')
    CREATE INDEX IX_AutomationCondition_Rule ON dbo.AutomationCondition (RuleID, SortOrder);
GO

IF OBJECT_ID(N'dbo.AutomationAction', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AutomationAction
    (
        ActionID        INT            IDENTITY(1,1) NOT NULL,
        RuleID          INT            NOT NULL,
        ActionType      VARCHAR(40)    NOT NULL,
        SortOrder       INT            NOT NULL,
        ParametersJson  NVARCHAR(MAX)  NOT NULL,
        ContinueOnError BIT            NOT NULL CONSTRAINT DF_AutomationAction_Continue DEFAULT (1),

        CONSTRAINT PK_AutomationAction PRIMARY KEY CLUSTERED (ActionID),
        CONSTRAINT FK_AutomationAction_Rule FOREIGN KEY (RuleID)
            REFERENCES dbo.AutomationRule (RuleID) ON DELETE CASCADE,
        CONSTRAINT UQ_AutomationAction_RuleOrder UNIQUE (RuleID, SortOrder),
        CONSTRAINT CK_AutomationAction_Type CHECK (ActionType IN
            ('CREATE_NOTIFICATION','CREATE_TASK','UPDATE_CLASS_STATUS','WRITE_AUDIT_LOG')),
        CONSTRAINT CK_AutomationAction_Json CHECK (ISJSON(ParametersJson) = 1),
        CONSTRAINT CK_AutomationAction_JsonObject
            CHECK (LEFT(LTRIM(ParametersJson), 1) = N'{')
    );

    PRINT N'  + AutomationAction';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationAction') AND name = N'IX_AutomationAction_Rule')
    CREATE INDEX IX_AutomationAction_Rule ON dbo.AutomationAction (RuleID, SortOrder);
GO

/* =====================================================================================
   3. Bảng lịch sử thực thi
   ===================================================================================== */
PRINT N'--- 3. Tạo bảng AutomationExecution / AutomationExecutionLog ---';

IF OBJECT_ID(N'dbo.AutomationExecution', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AutomationExecution
    (
        ExecutionID        BIGINT         IDENTITY(1,1) NOT NULL,
        RuleID             INT            NOT NULL,
        RuleNameSnapshot   NVARCHAR(150)  NOT NULL,
        TriggerType        VARCHAR(40)    NOT NULL,
        EntityType         VARCHAR(40)    NOT NULL CONSTRAINT DF_Execution_EntityType DEFAULT ('CLASS'),
        EntityID           VARCHAR(20)    NOT NULL,
        EventToken         VARCHAR(200)   NOT NULL,
        DedupKey           CHAR(64)       NOT NULL,
        Status             VARCHAR(20)    NOT NULL,
        ConditionsMatched  BIT            NOT NULL CONSTRAINT DF_Execution_Matched DEFAULT (0),
        ActionSuccessCount INT            NOT NULL CONSTRAINT DF_Execution_Success DEFAULT (0),
        ActionFailCount    INT            NOT NULL CONSTRAINT DF_Execution_Fail DEFAULT (0),
        StartedAt          DATETIME2(3)   NOT NULL CONSTRAINT DF_Execution_Started DEFAULT (SYSDATETIME()),
        FinishedAt         DATETIME2(3)   NULL,
        DurationMs         INT            NULL,
        TriggeredBy        VARCHAR(50)    NULL,
        ErrorMessage       NVARCHAR(2000) NULL,
        ContextJson        NVARCHAR(MAX)  NULL,

        CONSTRAINT PK_AutomationExecution PRIMARY KEY CLUSTERED (ExecutionID),
        CONSTRAINT FK_AutomationExecution_Rule FOREIGN KEY (RuleID)
            REFERENCES dbo.AutomationRule (RuleID),
        CONSTRAINT CK_AutomationExecution_Status CHECK
            (Status IN ('RUNNING','SUCCESS','PARTIAL','FAILED','SKIPPED')),
        CONSTRAINT CK_AutomationExecution_Context
            CHECK (ContextJson IS NULL OR ISJSON(ContextJson) = 1)
    );

    PRINT N'  + AutomationExecution';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationExecution') AND name = N'UX_AutomationExecution_Dedup')
    CREATE UNIQUE INDEX UX_AutomationExecution_Dedup
        ON dbo.AutomationExecution (RuleID, DedupKey);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationExecution') AND name = N'IX_AutomationExecution_Rule')
    CREATE INDEX IX_AutomationExecution_Rule
        ON dbo.AutomationExecution (RuleID, StartedAt DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationExecution') AND name = N'IX_AutomationExecution_Entity')
    CREATE INDEX IX_AutomationExecution_Entity
        ON dbo.AutomationExecution (EntityType, EntityID, StartedAt DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationExecution') AND name = N'IX_AutomationExecution_Status')
    CREATE INDEX IX_AutomationExecution_Status
        ON dbo.AutomationExecution (Status, StartedAt DESC);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationExecution') AND name = N'IX_AutomationExecution_Cooldown')
    CREATE INDEX IX_AutomationExecution_Cooldown
        ON dbo.AutomationExecution (RuleID, EntityType, EntityID, StartedAt DESC);
GO

IF OBJECT_ID(N'dbo.AutomationExecutionLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AutomationExecutionLog
    (
        LogID         BIGINT         IDENTITY(1,1) NOT NULL,
        ExecutionID   BIGINT         NOT NULL,
        StepType      VARCHAR(10)    NOT NULL,
        StepOrder     INT            NOT NULL,
        ConditionID   INT            NULL,
        ActionID      INT            NULL,
        StepName      NVARCHAR(200)  NOT NULL,
        IsSuccess     BIT            NOT NULL,
        ExpectedValue NVARCHAR(200)  NULL,
        ActualValue   NVARCHAR(200)  NULL,
        Message       NVARCHAR(1000) NULL,
        DurationMs    INT            NULL,
        CreatedAt     DATETIME2(3)   NOT NULL CONSTRAINT DF_ExecutionLog_Created DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_AutomationExecutionLog PRIMARY KEY CLUSTERED (LogID),
        CONSTRAINT FK_AutomationExecutionLog_Execution FOREIGN KEY (ExecutionID)
            REFERENCES dbo.AutomationExecution (ExecutionID) ON DELETE CASCADE,
        CONSTRAINT CK_AutomationExecutionLog_StepType CHECK (StepType IN ('CONDITION','ACTION')),
        CONSTRAINT CK_AutomationExecutionLog_Ref CHECK
        (
            (StepType = 'CONDITION' AND ConditionID IS NOT NULL AND ActionID IS NULL)
            OR
            (StepType = 'ACTION'    AND ActionID    IS NOT NULL AND ConditionID IS NULL)
        )
    );

    PRINT N'  + AutomationExecutionLog';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationExecutionLog') AND name = N'IX_AutomationExecutionLog_Exec')
    CREATE INDEX IX_AutomationExecutionLog_Exec
        ON dbo.AutomationExecutionLog (ExecutionID, StepType, StepOrder);
GO

/* =====================================================================================
   4. Bảng nhiệm vụ tự động
   ===================================================================================== */
PRINT N'--- 4. Tạo bảng AutomationTask ---';

IF OBJECT_ID(N'dbo.AutomationTask', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AutomationTask
    (
        TaskID            INT            IDENTITY(1,1) NOT NULL,
        ExecutionID       BIGINT         NULL,
        RuleID            INT            NOT NULL,
        Title             NVARCHAR(200)  NOT NULL,
        Description       NVARCHAR(1000) NULL,
        EntityType        VARCHAR(40)    NOT NULL CONSTRAINT DF_Task_EntityType DEFAULT ('CLASS'),
        EntityID          VARCHAR(20)    NOT NULL,
        AssignedToAccount VARCHAR(50)    NULL,
        AssignedToRole    VARCHAR(20)    NULL,
        Priority          VARCHAR(10)    NOT NULL CONSTRAINT DF_Task_Priority DEFAULT ('NORMAL'),
        Status            VARCHAR(20)    NOT NULL CONSTRAINT DF_Task_Status DEFAULT ('PENDING'),
        DueDate           DATE           NULL,
        CreatedAt         DATETIME2(0)   NOT NULL CONSTRAINT DF_Task_Created DEFAULT (SYSDATETIME()),
        CompletedAt       DATETIME2(0)   NULL,
        CompletedBy       VARCHAR(50)    NULL,
        IsOpen AS (CASE WHEN Status IN ('PENDING','IN_PROGRESS') THEN 1 ELSE 0 END) PERSISTED,

        CONSTRAINT PK_AutomationTask PRIMARY KEY CLUSTERED (TaskID),
        CONSTRAINT FK_AutomationTask_Execution FOREIGN KEY (ExecutionID)
            REFERENCES dbo.AutomationExecution (ExecutionID),
        CONSTRAINT FK_AutomationTask_Rule FOREIGN KEY (RuleID)
            REFERENCES dbo.AutomationRule (RuleID),
        CONSTRAINT FK_AutomationTask_Account FOREIGN KEY (AssignedToAccount)
            REFERENCES dbo.TaiKhoan (TenDangNhap),
        CONSTRAINT CK_AutomationTask_Status CHECK
            (Status IN ('PENDING','IN_PROGRESS','COMPLETED','CANCELLED')),
        CONSTRAINT CK_AutomationTask_Priority CHECK (Priority IN ('LOW','NORMAL','HIGH')),
        CONSTRAINT CK_AutomationTask_Role CHECK
            (AssignedToRole IS NULL OR AssignedToRole IN ('Admin','GiaoVien','HocVien')),
        CONSTRAINT CK_AutomationTask_Assignee CHECK
            (AssignedToAccount IS NOT NULL OR AssignedToRole IS NOT NULL),
        CONSTRAINT CK_AutomationTask_Completed CHECK
        (
            (Status = 'COMPLETED' AND CompletedAt IS NOT NULL)
            OR (Status <> 'COMPLETED' AND CompletedAt IS NULL)
        )
    );

    PRINT N'  + AutomationTask';
END
GO

/* Lọc theo Status (không dùng computed column IsOpen vì filtered index không cho phép). */
IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationTask') AND name = N'UX_AutomationTask_Open')
    CREATE UNIQUE INDEX UX_AutomationTask_Open
        ON dbo.AutomationTask (RuleID, EntityType, EntityID, Title)
        WHERE Status IN ('PENDING','IN_PROGRESS');
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationTask') AND name = N'IX_AutomationTask_Status')
    CREATE INDEX IX_AutomationTask_Status ON dbo.AutomationTask (Status, DueDate);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationTask') AND name = N'IX_AutomationTask_Assignee')
    CREATE INDEX IX_AutomationTask_Assignee ON dbo.AutomationTask (AssignedToRole, Status);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.AutomationTask') AND name = N'IX_AutomationTask_Account')
    CREATE INDEX IX_AutomationTask_Account ON dbo.AutomationTask (AssignedToAccount, Status);
GO

/* =====================================================================================
   5. Bảng thông báo dùng chung (cho action CREATE_NOTIFICATION)
   ===================================================================================== */
PRINT N'--- 5. Tạo bảng ThongBao ---';

IF OBJECT_ID(N'dbo.ThongBao', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ThongBao
    (
        ThongBaoID       BIGINT         IDENTITY(1,1) NOT NULL,
        TieuDe           NVARCHAR(200)  NOT NULL,
        NoiDung          NVARCHAR(1000) NOT NULL,
        MucDo            VARCHAR(10)    NOT NULL CONSTRAINT DF_ThongBao_MucDo DEFAULT ('INFO'),
        DoiTuongVaiTro   VARCHAR(20)    NULL,
        DoiTuongTaiKhoan VARCHAR(50)    NULL,
        EntityType       VARCHAR(40)    NULL,
        EntityID         VARCHAR(20)    NULL,
        RuleID           INT            NULL,
        ExecutionID      BIGINT         NULL,
        DaDoc            BIT            NOT NULL CONSTRAINT DF_ThongBao_DaDoc DEFAULT (0),
        CreatedAt        DATETIME2(0)   NOT NULL CONSTRAINT DF_ThongBao_Created DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_ThongBao PRIMARY KEY CLUSTERED (ThongBaoID),
        CONSTRAINT CK_ThongBao_MucDo CHECK (MucDo IN ('INFO','WARNING','ERROR')),
        CONSTRAINT CK_ThongBao_VaiTro CHECK
            (DoiTuongVaiTro IS NULL OR DoiTuongVaiTro IN ('Admin','GiaoVien','HocVien'))
    );

    PRINT N'  + ThongBao';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID(N'dbo.ThongBao') AND name = N'IX_ThongBao_DoiTuong')
    CREATE INDEX IX_ThongBao_DoiTuong
        ON dbo.ThongBao (DoiTuongVaiTro, DoiTuongTaiKhoan, DaDoc, CreatedAt DESC);
GO

/* =====================================================================================
   6. Quyền chức năng
   ===================================================================================== */
PRINT N'--- 6. Thêm mã quyền Automation ---';

MERGE dbo.ChucNang AS target
USING (VALUES
    ('AUTOMATION_XEM',    N'Xem tự động hóa',           N'Hệ thống', 0, 0, 28),
    ('AUTOMATION_QUANLY', N'Quản lý quy tắc tự động',   N'Hệ thống', 1, 0, 29),
    ('AUTOMATION_TASK',   N'Xử lý nhiệm vụ tự động',    N'Hệ thống', 0, 0, 30)
) AS source (MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu)
ON target.MaChucNang = source.MaChucNang
WHEN MATCHED THEN
    UPDATE SET TenChucNang     = source.TenChucNang,
               Nhom            = source.Nhom,
               ChiAdmin        = source.ChiAdmin,
               HoTroLocTheoChu = source.HoTroLocTheoChu,
               ThuTu           = source.ThuTu
WHEN NOT MATCHED THEN
    INSERT (MaChucNang, TenChucNang, Nhom, ChiAdmin, HoTroLocTheoChu, ThuTu)
    VALUES (source.MaChucNang, source.TenChucNang, source.Nhom,
            source.ChiAdmin, source.HoTroLocTheoChu, source.ThuTu);
GO

INSERT INTO dbo.PhanQuyen (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
SELECT m.MaChucNang, m.VaiTro, m.CoQuyen, m.ChiCuaMinh
FROM (VALUES
    ('AUTOMATION_XEM',  'GiaoVien', 1, 0),
    ('AUTOMATION_TASK', 'GiaoVien', 1, 0)
) AS m (MaChucNang, VaiTro, CoQuyen, ChiCuaMinh)
WHERE NOT EXISTS
(
    SELECT 1 FROM dbo.PhanQuyen p
    WHERE p.MaChucNang = m.MaChucNang AND p.VaiTro = m.VaiTro
);
GO

/* =====================================================================================
   7. KIỂM TRA SAU KHI CHẠY (chỉ đọc)
   ===================================================================================== */
PRINT N'--- 7. Kiểm tra kết quả ---';

SELECT t.name AS Bang,
       (SELECT COUNT(*) FROM sys.columns c WHERE c.object_id = t.object_id) AS SoCot,
       (SELECT COUNT(*) FROM sys.foreign_keys f WHERE f.parent_object_id = t.object_id) AS SoFK,
       (SELECT COUNT(*) FROM sys.indexes i
         WHERE i.object_id = t.object_id AND i.index_id > 0) AS SoIndex
FROM sys.tables t
WHERE t.name IN (N'AutomationRule', N'AutomationCondition', N'AutomationAction',
                 N'AutomationExecution', N'AutomationExecutionLog', N'AutomationTask', N'ThongBao')
ORDER BY t.name;

SELECT MaChucNang, TenChucNang, Nhom, ChiAdmin, ThuTu
FROM dbo.ChucNang
WHERE MaChucNang LIKE 'AUTOMATION%'
ORDER BY ThuTu;

SELECT MaLop, TrangThai, SiSoToiThieu, SiSoToiDa FROM dbo.LopHoc ORDER BY MaLop;
GO
