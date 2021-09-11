Binary file patcher

Usage:
patcher.exe [txt file name contains bytes] [file name to path]
[txt file name contains bytes] - Text file with 2 lines:
  First line: hex bytes to search
  Second line: hex bytes to replace
  example:
  mycrack.txt
  1B 80 90 90 E6
  1B 90 90 90 90
[file name to path] - any binary file name. This file will be overwritten.

Example:
patcher.exe mycrack.txt notepad.exe
