#define MyAppName "JDownloader Reach"
#define MyAppVersion "0.3.0"
#define MyAppPublisher "PCUser2021"
#define MyAppExeName "JDownloaderReach.exe"

[Setup]
SetupIconFile=..\src\ResolverReach\Assets\ResolverReach.ico
AppId={{A729354D-09C4-45BB-AF84-96CC4B2D2B08}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppComments=Independent control shell for JDownloader and Resolver Reach.
DefaultDirName={localappdata}\Programs\JDownloader Reach
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
ArchitecturesAllowed=arm64
ArchitecturesInstallIn64BitMode=arm64
OutputDir=..\artifacts\installer
OutputBaseFilename=JDownloaderReach-Setup-arm64
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\{#MyAppExeName}
CloseApplications=yes
RestartApplications=no
VersionInfoVersion=0.2.0.0
VersionInfoProductVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription=JDownloader Reach Windows ARM64 installer

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "..\artifacts\bundle\arm64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{autoprograms}\Resolver Reach"; Filename: "{app}\tools\ResolverReach\ResolverReach.exe"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch JDownloader Reach"; Flags: postinstall nowait skipifsilent
