# PhotonGame
Создать простую игру с клиент-серверным взаимодействием.
1) В игре должно быть 4 сцены. Одна главная сцена и 3 сцены боя.
2) На главной сцене можно выбрать персонажа, за которого мы будем играть (выбираем из трех любых),
UI выбор не принципиальнен, главное, чтобы мы видели персонажа, за которого будем играть.
Под каждым персонажем находится статистика его побед и поражений, которая сохраняется от сессии к сессии.
Справа от выбора персонажей находятся 3 кнопки с названиями сцен боя - нажатие на каждую из которых загружает выбранную сцену боя.
3) На сцене боя загружается выбранный нами персонаж и враг(любой).
Каждая сцена отличается от других каким-то простым способом - оформлением либо ланшафтом - разбросанные кубы и так далее.
Над каждым персонажем висит число, обозначающее его текущее здоровье.
4) Должен быть сервер, поднятый на Photon (http://photonengine.com), который отвечает за бой.
В бою камера должна показывать обоих персонажей.
При нажатии на персонажа отправляется запрос на сервер на разрешение ударить, после получения response от сервера удар либо наносится,
либо выскакивает окно о том что бить нельзя, так как персонаж мертв.
После этого появляется кнопка по которой мы возвращаемся на главную сцену, 
где у нас обновляется статистика по персонажам с учетом последнего боя.
5) Дополнительно - будет плюсом: с сервера раз в N минут приходит сигнал, 
по которому статистика побед и поражений персонажей обнуляется, 
при этом мы должны увидеть изменение статистики либо сразу, если находились онлайн в главной сцене, либо тогда, 
когда загрузилась главная сцена.
