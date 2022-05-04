# DutyBot
Бот для управления дежурными в команде. Разрабатывался для небольшой автоматизации рабочего процесса
## Настройка бота
В файле appsettings.json нужно указать следующие параметры:
* **Name** - название бота
* **Key** - токен для бота
* **ChatId** - id чата для отправки сообщений
* **Url** - адрес для веб-хука
* **Host** - хост на котором будет размещён ваш сервис
* **Always** - флаг, указывающий, должен ли крон отрабатывать всегда (значение "1") или только по рабочим дням
## Указание списка дежурных
В папке, где развёрнуто приложение, нужно создать два файла:
**users.yaml** - указать массив всех пользователей, участвующих в дежурстве. Идентификаторы указывать в виде порядкового номера пользователей:
```
- Id: 0
  Name: Имя пользователя
  Telegram: '@telegram'
  NextId: 1
- Id: 1
  Name: Имя пользователя 2
  Telegram: '@telegram2'
```
Пользователь без поля `NextId` - последний в списке. После этого дежурного следующим будет первый.

**current.yaml** - указать текущего дежурного: 
```
Id: 0
Name: Имя пользователя
Telegram: '@telegram'
NextId: 1
```

## Развёртывание
Проект предназначен для развёртывания на vps с nginx по стандартной [иструкции](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0).
Обратите внимание, что для веб-хуков телеграм требует подключения по https, поэтому не забудьте настроить ssl-сертификаты на вашем сервере