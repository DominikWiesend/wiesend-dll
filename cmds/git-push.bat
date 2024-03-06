@ECHO OFF&SETLOCAL && ;TITLE Commits all changes within the of local repository and pushes them to its remote. && ;rem ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
;FOR %%i in ("%~dp0.") DO FOR %%j in ("%%~dpi.") DO SET "RepositoryDirectory=%%~dpnxj" && ;rem ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
;SET "params=%*" && ;CD /d "%RepositoryDirectory%" && (IF EXIST "%temp%\getadmin.vbs" DEL "%temp%\getadmin.vbs") && fsutil dirty query %systemdrive% 1>nul 2>nul || (ECHO SET UAC = CreateObject^("Shell.Application"^) : UAC.ShellExecute "cmd.exe", "/k CD ""%~sdp0"" && ""%~s0"" %params%", "", "runas", 1 >> "%temp%\getadmin.vbs" && "%temp%\getadmin.vbs" && exit /B)
;COLOR 0A && ;rem ------------------ --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
;ECHO ^/^/ ------------------------- ------------------------------------------------------------------------ && ;rem -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
;ECHO ^/^/ #================================================================================================#
;ECHO ^/^/ # Author   ::    Dominik Wiesend    :: (dominik.wiesend@gmx.de)                                  #
;ECHO ^/^/ # Type     ::    Batch (cmd/bat)    :: (Windows Batch Command)                                   #
;ECHO ^/^/ # Date     ::    22.02.2024         :: (dd.MM.yyyy)                                              #
;ECHO ^/^/ #================================================================================================#
;ECHO ^/^/ # Version  ::    [1.0.0.0]          :: Initial Version (unchanged)                               #
;ECHO ^/^/ #================================================================================================#
;ECHO ^/^/ # Description:                                                                                   #
;ECHO ^/^/ #    This batch (commands) are used to commit all changes within                                 #
;ECHO ^/^/ #    the of local repository and pushes them to its remote.                                      #
;ECHO ^/^/ #================================================================================================#
;ECHO ^/^/ =============================================================================================== //
;ECHO ^/^/    Copyright(c) 2024 Dominik Wiesend. All rights reserved.
;ECHO ^/^/    
;ECHO ^/^/    Permission is hereby granted, free of charge, to any person obtaining a copy
;ECHO ^/^/    of this software and associated documentation files (the "Software"), to deal
;ECHO ^/^/    in the Software without restriction, including without limitation the rights
;ECHO ^/^/    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
;ECHO ^/^/    copies of the Software, and to permit persons to whom the Software is
;ECHO ^/^/    furnished to do so, subject to the following conditions:
;ECHO ^/^/    
;ECHO ^/^/    The above copyright notice and this permission notice shall be included in all
;ECHO ^/^/    copies or substantial portions of the Software.
;ECHO ^/^/    
;ECHO ^/^/    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
;ECHO ^/^/    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
;ECHO ^/^/    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
;ECHO ^/^/    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
;ECHO ^/^/    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
;ECHO ^/^/    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
;ECHO ^/^/    SOFTWARE.
;ECHO ^/^/ =============================================================================================== // 
;ECHO ^/^/ ------------------------- ------------------------------------------------------------------------
;ECHO ^/^/ ------------------------- ^>^> Repository Directory: %RepositoryDirectory%
;ECHO ^/^/ ------------------------- ^>^> Current Working Directory: %cd%
;ECHO ^/^/ ------------------------- ------------------------------------------------------------------------
;ECHO ^/^/ =============================================================================================== // 
;ECHO; && ;rem =========================================================================================== // 

;rem ============================= ========================================================================
;rem ============================= ========================================================================

;ECHO ^// ^<summary^>
;ECHO ^// Set the git config to support long paths (core.longpaths)
;ECHO ^// to avoid maximum file path limitation errors.
;ECHO ^// ^<^/summary^>
;ECHO ---------------------------- ------------------------------------------------------------------------
;ECHO Execute: git config --system core.longpaths true
;git config --system core.longpaths true
;ECHO ---------------------------- ------------------------------------------------------------------------
;ECHO done...

;rem ============================= ========================================================================
;rem ============================= ========================================================================
;ECHO; && ;ECHO ============================ ======================================================================== 
;ECHO ============================ ======================================================================== && ;ECHO;
;rem ============================= ========================================================================
;rem ============================= ========================================================================

;ECHO ^// ^<summary^>
;ECHO ^// Commits all changes within the of local
;ECHO ^// repository (working tree modifications).
;ECHO ^// ^</summary^>
;ECHO ---------------------------- ------------------------------------------------------------------------
;SET /p CommitMessage=Commit message (commit -m "Your Message"): 
;ECHO Execute: git add -A ^&^& git commit -m "%CommitMessage%"
;ECHO ---------------------------- ------------------------------------------------------------------------
;git add -A && git commit -m "%CommitMessage%"

;ECHO; && ;rem ------------------- ------------------------------------------------------------------------
;rem ----------------------------- ------------------------------------------------------------------------

;ECHO ^// ^<summary^>
;ECHO ^// Push all commits (working tree modifications) 
;ECHO ^// to its remote repository (git push).
;ECHO ^// ^</summary^>
;ECHO ---------------------------- ------------------------------------------------------------------------
;ECHO Execute: git push origin master
;ECHO ---------------------------- ------------------------------------------------------------------------
;git push origin master
;ECHO;

;rem ============================= ==========================================================================
;rem ============================= ==========================================================================
;rem ============================= ==========================================================================
;rem ============================= ==========================================================================
;ECHO ^/^/ =============================================================================================== // 
;ECHO ^/^/ =============================================================================================== // 
;ECHO ^/^/ ------------------------- ------------------------------------------------------------------------
;ECHO ^/^/ ------------------------- ------------------------------------------------------------------------
;ECHO ^/^/ ------------------------- ------------------------------------------------------------------------