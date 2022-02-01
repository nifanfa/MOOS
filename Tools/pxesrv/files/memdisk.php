<?php
#you can use mklink /j link c:\xxx\target to create a junction in your hosting folder
#will accept an absolute path or relative path 
#ex : c:/_quickpe or _pxe/quickpe - use // at the end of the path to include only dirs below the target
echo "#!ipxe\n";
echo "set boot-url http://\${next-server}\n";
#echo "show platform\n";
echo ":start\necho Boot menu\nmenu Selection\n";
if ($argc>1) {$root = $argv[1];}
if ($root=="") {
	$ini = parse_ini_file("config.ini");
	#current directory + root variable from config.ini
	$root=getcwd()."/".$ini['root']."//";
	#echo $root."\n";
}
$directory = new RecursiveDirectoryIterator($root);
$display = Array ( 'iso' ); #could be Array ( 'iso', 'raw' ) 
$count = 0;
foreach(new RecursiveIteratorIterator($directory) as $file)
{
    if (in_array(strtolower(array_pop(explode('.', $file))), $display))
        echo "item ". $count . " " . array_pop(explode("/", $file))  . "\n";
        $count += 1;
}
echo "item back back\n";
echo "choose os && goto \${os}\n";
$count = 0;
foreach(new RecursiveIteratorIterator($directory) as $file)
{
    if (in_array(strtolower(array_pop(explode('.', $file))), $display)){
		$filename= array_pop(explode("/", $file));
		$filename = ltrim($filename, chr(92)); 			
		echo ":". $count."\n";
		#memdisk, memdisk iso, or memdisk raw ?
		$pos = strpos($file, '.iso');
		if ($pos !== false) {
		echo "kernel \${boot-url}/memdisk iso\n";
		} else {
		echo "kernel \${boot-url}/memdisk raw\n";
		}
		echo "initrd \${boot-url}/".$filename."\n";
		echo "boot\n";
}
		$count += 1;
}
echo ":back\nchain menu-php.ipxe\n";
?>