# Лабораторная работа №29. REST API: контроллеры и маршруты

**ФИО:** Назаренко Алексей  
**Группа:** ИСП-233  
**Дата:** 08.05.2026

## Краткое описание работы

В лабораторной работе был создан Web API проект на ASP.NET Core. В проекте реализован контроллер `TasksController`, который обрабатывает REST-запросы для ресурса задач. Были изучены контроллеры, маршруты, DTO, Swagger UI, REST Client, HTTP-статусы, фильтрация, поиск, сортировка, статистика и настройка CORS.

## Структура проекта

```text
Lab29_RestAPI
├── img
├── TaskApi
│   ├── Controllers
│   │   └── TasksController.cs
│   ├── Models
│   │   ├── CreateTaskDto.cs
│   │   ├── TaskItem.cs
│   │   └── UpdateTaskDto.cs
│   ├── Properties
│   │   └── launchSettings.json
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── Program.cs
│   ├── requests.http
│   └── TaskApi.csproj
├── .editorconfig
├── .gitignore
└── README.md
```

## Реализованные маршруты

| Метод | URL | Описание |
| --- | --- | --- |
| GET | `/api/tasks` | Получить все задачи |
| GET | `/api/tasks?completed=false` | Получить задачи с фильтром по статусу |
| GET | `/api/tasks/{id}` | Получить задачу по id |
| POST | `/api/tasks` | Создать новую задачу |
| PUT | `/api/tasks/{id}` | Обновить задачу |
| DELETE | `/api/tasks/{id}` | Удалить задачу |
| PATCH | `/api/tasks/{id}/toggle` | Изменить статус выполнения задачи |
| GET | `/api/tasks/search?query=API` | Найти задачи по заголовку или описанию |
| GET | `/api/tasks/priority/{level}` | Получить задачи по приоритету |
| GET | `/api/tasks/stats` | Получить статистику по задачам |
| GET | `/api/tasks/sorted?by=title` | Получить отсортированный список задач |

## Итоговая таблица ASP.NET Core Controller-based API

| Аспект | ASP.NET Core Controllers |
| --- | --- |
| Маршруты | `[HttpGet]` атрибут над методом |
| Группировка маршрутов | Класс-контроллер |
| Базовый URL | `[Route("api/[controller]")]` |
| Параметр пути | `(int id)` — параметр метода |
| Параметр запроса | `[FromQuery] bool? completed` |
| Тело запроса | `[FromBody] CreateTaskDto dto` |
| Ответ 200 | `return Ok(data)` |
| Ответ 201 | `return CreatedAtAction(...)` |
| Ответ 404 | `return NotFound(...)` |
| Ответ 204 | `return NoContent()` |
| Типизация данных | Строгая (C#) |
| Документация | Swagger — устанавливается отдельно |

## Главные выводы

1. REST — не протокол, а архитектурный стиль. Те же принципы работают с любым языком и фреймворком.
2. Контроллер в ASP.NET Core = Router в Express, только с автоматической документацией и строгой типизацией.
3. DTO защищает API от некорректных данных: клиент передаёт только то, что сервер разрешает принять.
4. Swagger UI позволяет тестировать API без Postman и без написания тестового JavaScript-кода.
5. Правильные HTTP-статусы — часть контракта API. Клиент должен понимать, что произошло, по коду ответа.

## Итоговая таблица: что изучили в лабораторной

| Концепция / команда | Описание |
| --- | --- |
| `dotnet new webapi` | Создать проект Web API с контроллерами и Swagger |
| `[ApiController]` | Атрибут, включающий автоматическую валидацию и обработку ошибок |
| `[Route("api/[controller]")]` | Базовый URL для всех маршрутов контроллера |
| `[HttpGet]`, `[HttpPost]` и т.д. | Атрибуты, задающие HTTP-метод маршрута |
| `[FromBody]` | Данные берутся из тела JSON-запроса |
| `[FromQuery]` | Данные берутся из строки запроса |
| `ActionResult<T>` | Тип возврата: данные или HTTP-статус |
| `Ok(data)` | HTTP 200 с данными |
| `CreatedAtAction(...)` | HTTP 201 с заголовком Location |
| `NotFound(...)` | HTTP 404 |
| `BadRequest(...)` | HTTP 400 |
| `NoContent()` | HTTP 204 |
| DTO | Отдельный класс для входящих данных без служебных полей |
| Swagger UI | Автодокументация и тестирование API в браузере |
| REST Client | Тестирование API через файлы `.http` прямо в редакторе |
| CORS | Политика разрешений для запросов с других доменов и портов |
