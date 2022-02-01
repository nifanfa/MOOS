Option Explicit 
on error resume next
' *********************     functions ***********************

Sub Include (strFile)
	'Create objects for opening text file
	dim objFSO
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	dim objTextFile
	Set objTextFile = objFSO.OpenTextFile(strFile, 1)

	'Execute content of file.
	ExecuteGlobal objTextFile.ReadAll

	'CLose file
	objTextFile.Close

	'Clean up
	Set objFSO = Nothing
	Set objTextFile = Nothing
End Sub

Sub getInfo(pCurrentDir,s)
dim aItem
For Each aItem In pCurrentDir.Files
   'wscript.Echo aItem.Name
   If LCase(Right(Cstr(aItem.Name), 3)) = "iso" Then
    c=c+1
    if s=1 then wscript.echo "item " & c & chr(9) & aItem.name 
	dim path
	path=replace(replace(aItem.path,strweb,""),"\","/")
	path=replace(path,"#","%23")
	path=replace(path," ","%20")
	if s=2 then 
		wscript.echo ":" & c 
		wscript.echo "kernel ${boot-url}/memdisk iso"
		wscript.echo "initrd ${boot-url}/" & path
		wscript.echo "boot"
	end if
   End If
Next

For Each aItem In pCurrentDir.SubFolders
   'wscript.Echo aItem.Name & " passing recursively"
   call getInfo(aItem,s)
Next

End Sub

'************************************************
dim fso
Set FSO = CreateObject("Scripting.FileSystemObject")

dim ScriptPath
ScriptPath= Wscript.ScriptFullName
dim objFile
Set objFile = FSO.GetFile(ScriptPath)
dim ScriptFolder
ScriptFolder = FSO.GetParentFolderName(objFile) 
'wscript.echo "strFolder:" & strFolder

Include(ScriptFolder & "\ini.vbs")

dim CurrentDirectory
CurrentDirectory = fso.GetAbsolutePathName(".")
'wscript.echo "CurrentDirectory:" & CurrentDirectory

dim root
root= ReadIni( CurrentDirectory & "\config.ini", "dhcp", "root" )
'wscript.echo "root:" & root

dim strweb
if instr(root,"\")>0 then strweb = root
if instr(root,"\")=0 then strweb = CurrentDirectory & "\" & root
'wscript.echo "strweb:" & strweb

dim objDir
Set objDir = FSO.GetFolder(strweb)
'***************** lets generate the ipxe script **************

wscript.echo "#!ipxe"
wscript.echo "set boot-url http://${next-server}"
wscript.echo "set iscsi-server ${next-server}"
wscript.echo "set iqn iqn.2008-08.com.starwindsoftware:target1"
wscript.echo "set iscsi-target iscsi:${iscsi-server}::::${iqn}"
wscript.echo "set menu-timeout 5000"
wscript.echo "set submenu-timeout ${menu-timeout}"
wscript.echo "isset ${menu-default} || set menu-default exit"
wscript.echo ":start"
wscript.echo "menu Welcome to iPXE's Boot Menu"
'wscript.echo "item"
'wscript.echo "item --gap -- ------------------------- ISO ------------------------------"

dim c
c=0
call getInfo(objDir,1)

wscript.echo "item top back to top menu"
wscript.echo "choose --default exit --timeout 30000 target && goto ${target}"

c=0
call getInfo(objDir,2)


wscript.echo ":top chain menu-vbs.ipxe"
wscript.echo "goto start"