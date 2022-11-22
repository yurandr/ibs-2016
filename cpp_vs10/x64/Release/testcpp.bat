cd %~dp0
cpp.exe "testcpp.txt" "(\w+)-(\d+)-(\w+)-(\d+)" "$1*$2*$3*$4"
pause