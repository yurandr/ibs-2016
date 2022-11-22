cd %~dp0
cpp.exe "testcpp2.txt" "sd (.*?) \[sda\] (.*)" "time $1 message $2"
pause