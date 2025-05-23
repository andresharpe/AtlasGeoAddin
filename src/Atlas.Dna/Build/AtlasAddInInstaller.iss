#define BuildOutputDir GetEnv("OutDir")

#ifndef AppVersion
#define AppVersion "0.0.0-dev"
#endif

[Setup]
AppName=Atlas Excel Add-In
AppVersion={#AppVersion}
DefaultDirName={userappdata}\AtlasAddIn
DefaultGroupName=Atlas AddIn
OutputDir={#BuildOutputDir}
OutputBaseFilename=AtlasAddInInstaller-v{#AppVersion}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=lowest
UninstallDisplayIcon={app}\Atlas.Dna-AddIn64.xll
UninstallDisplayName=Atlas Excel Add-In
SignTool=signtool $f


[Files]
Source: "{#BuildOutputDir}\publish\Atlas.Dna-AddIn64-packed.xll"; DestDir: "{app}"; Flags: ignoreversion signonce

[Code]
procedure AddOrUpdateExcelAddinKey();
var
  i: Integer;
  value, addinPath, keyName: String;
  exists: Boolean;
begin
  exists := False;
  addinPath := '/R "' + ExpandConstant('{app}') + '\Atlas.Dna-AddIn64-packed.xll"';

  for i := 0 to 20 do
  begin
    if i = 0 then
      keyName := 'OPEN'
    else
      keyName := 'OPEN' + IntToStr(i);

    if RegQueryStringValue(HKEY_CURRENT_USER, 'Software\Microsoft\Office\16.0\Excel\Options', keyName, value) then
    begin
      if Pos('Atlas.Dna-AddIn64.xll', value) > 0 then
      begin
        exists := True;
        break;
      end;
    end
    else
    begin
      RegWriteStringValue(HKEY_CURRENT_USER, 'Software\Microsoft\Office\16.0\Excel\Options', keyName, addinPath);
      exists := True;
      break;
    end;
  end;

  if not exists then
    MsgBox('Could not register Excel add-in. All OPEN slots are full.', mbError, MB_OK);
end;

procedure RemoveExcelAddinKey();
var
  i: Integer;
  value, keyName: String;
begin
  for i := 0 to 20 do
  begin
    if i = 0 then
      keyName := 'OPEN'
    else
      keyName := 'OPEN' + IntToStr(i);

    if RegQueryStringValue(HKEY_CURRENT_USER, 'Software\Microsoft\Office\16.0\Excel\Options', keyName, value) then
    begin
      if Pos('Atlas.Dna-AddIn64.xll', value) > 0 then
      begin
        RegDeleteValue(HKEY_CURRENT_USER, 'Software\Microsoft\Office\16.0\Excel\Options', keyName);
      end;
    end;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
    AddOrUpdateExcelAddinKey();
end;

procedure DeinitializeUninstall();
begin
  RemoveExcelAddinKey();
end;

