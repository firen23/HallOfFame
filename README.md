Hall Of Fame (Компетенции сотрудников)
========================
Реализация back-end части одностраничного web-приложения для просмотра и редактирования навыков персонала.

В данной реализации предполагается, что навыки не могут быть утрачены.

Требования:
------------------------
* [MSSQL](https://www.microsoft.com/ru-ru/sql-server/sql-server-downloads)

Важно:
-----------------------
Добавьте в файл **appsettings.json** конфигурацию подключения к MSSQL в формате:
> Server=myServerName\myInstanceName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;

Например:
> "ConnectionStrings": {  
>  "DefaultConnection": "Server=myServerName\\myInstanceName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword"  
>},

_Примечание: не забудьте экранировать символы "\\"_

Миграции:
-----------------------
Для приведения структуры БД к кодовой базе нужно применить миграцию:
>dotnet ef database update