@echo off
rem 关闭回显命令

cd C:\Program Files\Internet Explorer\
rem 改变当前目录到IE所在目录

start iexplore.exe "https://www.zhongxiaole.net/SPManager/sPWebAPI/Card/GetCsharpNews"
rem 启动IE 执行加载路由连接程序

ping 127.1 -n 30 >nul 
rem 延时30秒
taskkill /im iexplore.exe /f
rem 关闭IE