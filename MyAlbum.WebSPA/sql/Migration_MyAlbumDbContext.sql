USE MyAlbum
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Users] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(max) NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Albums] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [AuthorId] nvarchar(450) NULL,
    CONSTRAINT [PK_Albums] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Albums_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Photos] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [FilePath] nvarchar(max) NULL,
    [AlbumId] int NULL,
    [AuthorId] nvarchar(450) NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Photos_Albums_AlbumId] FOREIGN KEY ([AlbumId]) REFERENCES [Albums] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Photos_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NULL,
    [PhotoId] int NULL,
    [AuthorId] nvarchar(450) NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Comments_Photos_PhotoId] FOREIGN KEY ([PhotoId]) REFERENCES [Photos] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [PhotoCategories] (
    [PhotoId] int NOT NULL,
    [CategoryId] int NOT NULL,
    CONSTRAINT [PK_PhotoCategories] PRIMARY KEY ([PhotoId], [CategoryId]),
    CONSTRAINT [FK_PhotoCategories_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PhotoCategories_Photos_PhotoId] FOREIGN KEY ([PhotoId]) REFERENCES [Photos] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Albums_AuthorId] ON [Albums] ([AuthorId]);

GO

CREATE INDEX [IX_Comments_AuthorId] ON [Comments] ([AuthorId]);

GO

CREATE INDEX [IX_Comments_PhotoId] ON [Comments] ([PhotoId]);

GO

CREATE INDEX [IX_PhotoCategories_CategoryId] ON [PhotoCategories] ([CategoryId]);

GO

CREATE INDEX [IX_Photos_AlbumId] ON [Photos] ([AlbumId]);

GO

CREATE INDEX [IX_Photos_AuthorId] ON [Photos] ([AuthorId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200114020422_InitialModel', N'3.1.5');

GO

ALTER TABLE [Photos] ADD [Height] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Photos] ADD [Width] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200120112149_AddPhotoWidthHeight', N'3.1.5');

GO

SET IDENTITY_INSERT Categories ON

GO

INSERT INTO Categories (Id, Name) VALUES (1, 'aeroplane')

GO

INSERT INTO Categories (Id, Name) VALUES (2, 'bicycle')

GO

INSERT INTO Categories (Id, Name) VALUES (3, 'bird')

GO

INSERT INTO Categories (Id, Name) VALUES (4, 'boat')

GO

INSERT INTO Categories (Id, Name) VALUES (5, 'bottle')

GO

INSERT INTO Categories (Id, Name) VALUES (6, 'bus')

GO

INSERT INTO Categories (Id, Name) VALUES (7, 'car')

GO

INSERT INTO Categories (Id, Name) VALUES (8, 'cat')

GO

INSERT INTO Categories (Id, Name) VALUES (9, 'chair')

GO

INSERT INTO Categories (Id, Name) VALUES (10, 'cow')

GO

INSERT INTO Categories (Id, Name) VALUES (11, 'diningtable')

GO

INSERT INTO Categories (Id, Name) VALUES (12, 'dog')

GO

INSERT INTO Categories (Id, Name) VALUES (13, 'horse')

GO

INSERT INTO Categories (Id, Name) VALUES (14, 'motorbike')

GO

INSERT INTO Categories (Id, Name) VALUES (15, 'person')

GO

INSERT INTO Categories (Id, Name) VALUES (16, 'pottedplant')

GO

INSERT INTO Categories (Id, Name) VALUES (17, 'sheep')

GO

INSERT INTO Categories (Id, Name) VALUES (18, 'sofa')

GO

INSERT INTO Categories (Id, Name) VALUES (19, 'train')

GO

INSERT INTO Categories (Id, Name) VALUES (20, 'tvmonitor')

GO

SET IDENTITY_INSERT Categories OFF

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200130075657_SeedDatabase', N'3.1.5');

GO

ALTER TABLE [Photos] ADD [CenterLat] float NULL;

GO

ALTER TABLE [Photos] ADD [CenterLng] float NULL;

GO

ALTER TABLE [Photos] ADD [LocLat] float NULL;

GO

ALTER TABLE [Photos] ADD [LocLng] float NULL;

GO

ALTER TABLE [Photos] ADD [MapFilePath] nvarchar(max) NULL;

GO

ALTER TABLE [Photos] ADD [MapZoom] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200421013837_AddPhotoMapProps', N'3.1.5');

GO

ALTER TABLE [Comments] ADD [ParentId] int NULL;

GO

CREATE INDEX [IX_Comments_ParentId] ON [Comments] ([ParentId]);

GO

ALTER TABLE [Comments] ADD CONSTRAINT [FK_Comments_Comments_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Comments] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200527031325_AddCommentReply', N'3.1.5');

GO

ALTER TABLE [Users] ADD [CreatedDate] datetime2 NULL DEFAULT (getutcdate());

GO

ALTER TABLE [Users] ADD [ModifiedDate] datetime2 NULL;

GO

ALTER TABLE [Photos] ADD [CreatedDate] datetime2 NULL DEFAULT (getutcdate());

GO

ALTER TABLE [Photos] ADD [ModifiedDate] datetime2 NULL;

GO

ALTER TABLE [Comments] ADD [CreatedDate] datetime2 NULL DEFAULT (getutcdate());

GO

ALTER TABLE [Comments] ADD [ModifiedDate] datetime2 NULL;

GO

ALTER TABLE [Albums] ADD [CreatedDate] datetime2 NULL DEFAULT (getutcdate());

GO

ALTER TABLE [Albums] ADD [ModifiedDate] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200621145803_AddDateTime', N'3.1.5');

GO


