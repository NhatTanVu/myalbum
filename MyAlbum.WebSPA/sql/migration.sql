/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Albums]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Albums](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[AuthorId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[PhotoId] [int] NULL,
	[AuthorId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhotoCategories]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhotoCategories](
	[PhotoId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_PhotoCategories] PRIMARY KEY CLUSTERED 
(
	[PhotoId] ASC,
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Photos]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Photos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[FilePath] [nvarchar](max) NULL,
	[AlbumId] [int] NULL,
	[AuthorId] [nvarchar](450) NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
 CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/6/2020 10:24:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20200114020422_InitialModel', N'3.1.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20200120112149_AddPhotoWidthHeight', N'3.1.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20200130075657_SeedDatabase', N'3.1.0')
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'aeroplane')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'bicycle')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (3, N'bird')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (4, N'boat')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (5, N'bottle')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (6, N'bus')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (7, N'car')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (8, N'cat')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (9, N'chair')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (10, N'cow')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (11, N'diningtable')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (12, N'dog')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (13, N'horse')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (14, N'motorbike')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (15, N'person')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (16, N'pottedplant')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (17, N'sheep')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (18, N'sofa')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (19, N'train')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (20, N'tvmonitor')
SET IDENTITY_INSERT [dbo].[Categories] OFF
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName]) VALUES (N'{559d2e16-f464-4438-85d1-c8ef7776ab27}', N'Anonymous', N'Anonymous', N'')
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Albums_AuthorId]    Script Date: 2/6/2020 10:24:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Albums_AuthorId] ON [dbo].[Albums]
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Comments_AuthorId]    Script Date: 2/6/2020 10:24:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_AuthorId] ON [dbo].[Comments]
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_PhotoId]    Script Date: 2/6/2020 10:24:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_PhotoId] ON [dbo].[Comments]
(
	[PhotoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PhotoCategories_CategoryId]    Script Date: 2/6/2020 10:24:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_PhotoCategories_CategoryId] ON [dbo].[PhotoCategories]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Photos_AlbumId]    Script Date: 2/6/2020 10:24:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Photos_AlbumId] ON [dbo].[Photos]
(
	[AlbumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Photos_AuthorId]    Script Date: 2/6/2020 10:24:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Photos_AuthorId] ON [dbo].[Photos]
(
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Photos] ADD  DEFAULT ((0)) FOR [Height]
GO
ALTER TABLE [dbo].[Photos] ADD  DEFAULT ((0)) FOR [Width]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Users_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Users_AuthorId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Photos_PhotoId] FOREIGN KEY([PhotoId])
REFERENCES [dbo].[Photos] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Photos_PhotoId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users_AuthorId]
GO
ALTER TABLE [dbo].[PhotoCategories]  WITH CHECK ADD  CONSTRAINT [FK_PhotoCategories_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhotoCategories] CHECK CONSTRAINT [FK_PhotoCategories_Categories_CategoryId]
GO
ALTER TABLE [dbo].[PhotoCategories]  WITH CHECK ADD  CONSTRAINT [FK_PhotoCategories_Photos_PhotoId] FOREIGN KEY([PhotoId])
REFERENCES [dbo].[Photos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhotoCategories] CHECK CONSTRAINT [FK_PhotoCategories_Photos_PhotoId]
GO
ALTER TABLE [dbo].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_Albums_AlbumId] FOREIGN KEY([AlbumId])
REFERENCES [dbo].[Albums] ([Id])
GO
ALTER TABLE [dbo].[Photos] CHECK CONSTRAINT [FK_Photos_Albums_AlbumId]
GO
ALTER TABLE [dbo].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_Users_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Photos] CHECK CONSTRAINT [FK_Photos_Users_AuthorId]
GO
