#define MyAppName "Resolver Reach"
#ifndef MyAppVersion
  #define MyAppVersion "2.5.0"
#endif
#ifndef MyAppArchitecture
  #define MyAppArchitecture "x64compatible"
#endif
#ifndef MyAppOutputName
  #define MyAppOutputName "x64"
#endif
#define MyAppPublisher "PCUser2021"
#define MyAppExeName "ResolverReach.exe"

[Setup]
SetupIconFile=..\src\ResolverReach\Assets\ResolverReach.ico
AppId={{1F1CBB7B-5A0C-4E08-9EF2-5DBEFA683D38}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={localappdata}\Programs\Resolver Reach
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
ArchitecturesAllowed={#MyAppArchitecture}
ArchitecturesInstallIn64BitMode={#MyAppArchitecture}
OutputDir=..\artifacts\installer
OutputBaseFilename=ResolverReach-Setup-{#MyAppOutputName}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\{#MyAppExeName}
CloseApplications=yes
RestartApplications=no

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "..\artifacts\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch Resolver Reach"; Flags: postinstall nowait skipifsilent
