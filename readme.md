## Binary file patcher

#### Usage:
patcher.exe [txt file name contains bytes] [file name to path]
- ###### [txt file name contains bytes] 
    Text file with 2 lines:
    
    First line: hex bytes to search
    
    Second line: hex bytes to replace
    
    example #1 (without mask):
    
    mycrack.txt
    
    1B 80 90 90 E6
    
    1B 90 90 90 90  

    example #2 (with search pattern mask):
    
    mycrack.txt
    
    1B 80 90 ?? E6
    
    1B 90 90 90 90  

    example #3 (with replace pattern mask):
    
    mycrack.txt
    
    1B 80 90 90 E6
    
    ?? 90 ?? ?? ??  

- ###### [file name to path] 
  any binary file name. This file will be overwritten.

Usage example: 

     patcher.exe mycrack.txt notepad.exe
