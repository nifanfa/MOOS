<?php
#you can use mklink /j link c:\xxx\target to create a junction in your hosting folder
#will accept an absolute path or relative path 
#ex : c:/_quickpe or _pxe/quickpe - use // at the end of the path to include only dirs below the target
echo "#!ipxe\n";
echo "set boot-url http://\${next-server}\n";
echo ":start\necho Boot menu\nmenu Selection\n";
if ($argc>1) {$root = $argv[1];}
if ($root=="") {
	$ini = parse_ini_file("config.ini");
	#current directory + root variable from config.ini
	$root=getcwd()."/".$ini['root']."//";
	#echo $root."\n";
}
$directory = new RecursiveDirectoryIterator($root);
$display = Array ( 'iso' );
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
		#$pos = strpos($file, ':');
		#if ($pos !== false) {
		##absolute path
		#echo ":". $count ."\nsanboot \${boot-url}/". substr($file,3) . "\ngoto start\n";	
		#	} else {
		##relative path
		#echo ":". $count ."\nsanboot \${boot-url}/". substr($file,0) . "\ngoto start\n";		
		#}
		echo ":". $count ."\nsanboot --no-describe \${boot-url}/".$filename. "\ngoto start\n";		
}
		$count += 1;
}
echo ":back\nchain menu-php.ipxe\n";
?>