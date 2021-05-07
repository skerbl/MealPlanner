#$assemblyPath=$args[0]
write-host Running post-build script:

If(test-path settings\schema_settings.xsd)
{
    write-host Cleaning up schema
    remove-item settings\schema_settings.xsd
}

If(!(test-path settings))
{
    write-host Creating settings folder
    New-Item -ItemType Directory -Force -Path settings
}

& "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\xsd.exe" MPData.dll /type:MPData.UserSettings -outputdir:settings

If(test-path settings\schema0.xsd)
{
    rename-item -Path settings\schema0.xsd -NewName schema_settings.xsd
}