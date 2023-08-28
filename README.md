# Practika1

Этот проект был создан в рамках моей инициативы которую я назвал «практикой». Суть была проста. Мы с командой выбираем темы, над которыми хотим поработать и придумываем проект под них. Как ясно из названия, цель была именно в получении практического опыта и отработке навыков.

Для себя я определил, что хочу работать с процедурной генерацией по заранее заданному сиду и алгоритмом поиска пути. Подробнее про то как проходил процесс разработки можно почитать в дневнике ниже. Здесь же я отмечу что затея оправдалась на все сто. Даже в рамках гейм джемов редко, когда получалось столь подробно поработать над такими объёмными темами. Я много чего для себя вынес и точно проведу ещё одну практику.

## Дневник практики
- [English](#en)

- [День 1](###День-1)
- [День 2](###День2)
- [День 3](###День3)
- [День 4](###День4)
- [День 5-6](###День5–6-Финалпервойпрактики!)
- [Итоги](###Итак,самоевремяподвестиитоги.)

# EN

Решили делать 2-д пошаговую тактику с ориентиром на геймплей The last spell, Stoneshard и Into the Breach. В связи с этим решил не натягивать сову на глобус и отказался от ECS, поскольку она слабо подходит для пошаговых игр, ведь в них почти всё происходит в порядке очереди и по конкретным событиям, а не каждый кадр.

Взамен этого решил наконец попытаться разобраться с MVC хотя он для меня всё ещё звучит крайне размыто (а именно где в Unity следует проводить границу между Model и View), но возможно на практике станет понятнее.

Собственно, решил начать с генератора карты по которой будут перемещаться персонажи и строить всё остальное исходя из этого. От генератора требовалось: Создавать карту по seed-у; Иметь возможность генерировать разные типы местности с разными весами (в дальнейшем с возможностью настройки параметров этой самой местности); Генерировать "красивое" соединение между разными типами местности. 
Пока получилось вот: https://youtu.be/gHdvAlq1bqc

Пока это просто раскрашенная карта шумов с небольшим улучшением в виде заполнения "швов" между зонами. Сама форма зон мне не нравится и над ними ещё явно придётся поработать (хотя может "второй слой генерации", где будут создаваться всякие украшения по типу, стен, деревьев, кустов и прочего, всё исправит)

### День 2

Ух... Думал сегодня заняться подготовкой сгенерированной карты для того чтобы её можно было использовать для поиска пути и прочего веселья (расстановка стоимости клетки, занята ли клетка или нет и т.п.), но по итогу продвинулся не так далеко (относительно реализации всех механик). Зато много чего переписал из уже готового:

Во-первых, заметил то что иногда генератор карты шумов выдавал значения меньше нуля и больше 1 хотя Unity мануал говорит, что не должен. Не стал копаться в тонкостях работы самого генератора шума и просто ввёл строгие рамки (к тому же крайние значения мне на деле не так уж и важны, так что можно не париться)

Во-вторых, добавил второй заход для генератора швов. Теперь в первом заходе он "заделывает дыры" и уже на второй генерирует соединение.

В-третьих, переписал алгоритм внутри генератора швов отвечавшего за проверку соседних тайлов на соответствие текущему. Теперь весь код влезает в строк 20 взамен 200 - спасибо Логике (науке, а не моей логике).

Четвёртое, наиболее важное - изменил алгоритм выбора тайла для подстановки в качестве соединительного. Для этого решил впервые написать кастомное PropertyDrawer чтобы корректно отображать правила, по которым будет выбираться тайл: https://youtu.be/iC5rBiM1bug
После этого я изначально думал задать для тайлов Dictionary с множеством ключей, но к своему несчастью забыл, что в нём есть всего один ключ. Пытался найти BruteForce-ное решение в виде поиска лазейки как передать внутрь ключа сразу несколько ключей, либо простого способа написать свой Dictionary с несколькими ключами. 

К моему счастью мне на помощь пришла старая добрая информатика ещё школьного курса - а именно двоичная система. Дело в том, что значение, которое я пытался сохранить было 8 булевых переменных и если вспомнить что они сами по себе являются битами, то проблемы в том, чтобы представить их в виде числа в двоичной системе - вообще никаких. И самое прекрасное что каждой комбинации будет строго соответствовать только один ключ.

Который раз убеждаюсь в КРАЙНЕЙ полезности знания таких вещей как: алгоритмы, математика, информатика, физика и прочее, так что, если вы сейчас учитесь - рекомендую не пропускать эти темы, они сильно сэкономит ваши нервы и время.

Собственно, плюсы такого алгоритма поиска на лицо: моментальный поиск нужного тайла (что очень значимо для генерации карты), быстрый и относительно простой ввод новых правил для генерации соединительных тайлов. Разве что я бы хотел переписать PropertyDrawer добавив возможность выбора "квантового" значения, когда значение может быть, как да, так и нет что позволит ещё сильнее упростить создание новых правил, поскольку сейчас требуется учесть ВСЕ возможные комбинации при которых стоит выбрать этот конкретный тайл что даже звучит крайне муторно (хотя всё ещё лучше, чем было).

Ну и да, всё же основную задачу на день я не совсем забыл и добавил некоторые заготовки для игрового поля, с помощью которых будет происходить весь остальной процесс взаимодействия с полем. Так что думаю завтра наконец приступлю к первым реализациям алгоритмов поиска и прочей весёлой штуки... как только посплю. А ведь я планировал не сидеть до 6 утра, ну да ладно - главное, что весело.

### День 3

Ещё раз спасибо за отзыв, постарался учесть некоторые из перечисленных моментов. В целом сегодня снова по большей части занимался переписыванием уже имеющегося, но есть пару обновлений:

Как и собирался - добавил возможность выбора "квантовых" значений для правил генерации что значительно упростило их создание: https://youtu.be/lB3YFf-yJfg
Ну а также настроил их чуть более симпатичный вывод в инспектор.

А также наконец приступил к написанию алгоритма поиска. Не стал придумывать велосипед и попытался повторить A* алгоритм, но к сожалению, представить логику работы и написать сам алгоритм две разных вещи (пусть и довольно близкие). Первый блин пошёл комом, и я запутался в том, что понаписал, так что решил взять за ориентир вот эту статью: https://habr.com/ru/articles/513158/
И уже опираясь на неё попробовать снова. А что из этого вышло узнаем в следующем выпуске, а сейчас спать)

### День 4

Алгоритм поиска пути, то что я так хотел сделать - готово... вернее его черновая версия, поскольку в текущей версии алгоритма явно есть что переделать, но давайте по порядку.

В предыдущем посте я написал, что собираюсь использовать алгоритм A* для поиска пути и по началу это на самом деле было так... пока умные мысли наконец не догнали меня. Дело в том что A* предполагается для быстрого единоразового поиска пути до конкретной точки и это могло бы быть тем что мне нужно если бы не одно НО. В тактических играх обычно у нас есть такое понятие как "очки передвижения" из которых вытекает следующее рассуждение: 

В свой ход мы можем добраться до ограниченного набора клеток => Игрок имеет возможность выбрать любую клетку при условии, что она содержится в этом наборе => Нам нужен алгоритм, который сможет сгенерировать такой набор клеток и при этом сохранить путь до каждой из них чтобы нам не пришлось считать по новой при каждом "перевыборе клетки".

Из этого вытекает что при всех плюсах A* для нашей задачи он не подходит (во всяком случае я не стал пробовать перекроить его под это). Ему на замену пришёл волновой алгоритм... вернее моя пародия на него, поскольку в тот миг меня озарила "светлая мысль" и я принялся реализовывать идею.

По итогу вышел относительно тупой "перебиратор" всех возможных вариантов пути с запоминанием наиболее оптимального маршрута для каждой конкретной клетки: https://youtu.be/HZI0DQ6t3l8

Чуть подробнее - алгоритм перебирает все доступные варианты перехода из клетки и для каждого считает стоимость прохода, а также запоминает откуда пришли в эту клетку и сколько очков перемещения осталось и так повторяет пока не закончатся очки движения. При необходимости он пере запоминает значение (если удалось найти более короткий путь - т.е. тот у которого осталось больше очков перемещения).

Собственно, вот и всё. Пока что у алгоритма явная проблема с тем самым "пере запоминанием" на которое приходится большая часть всех расчётов что не есть гуд и это будет нужно поправить (например, наконец добавив ту самую итеративность что есть в оригинальном волновом алгоритме, когда мы перебираем слой за слоем), но это уже проблемы завтрашнего меня. 

### День 5 – 6 - Финал первой практики!

Начнём по порядку:

Во-первых, была улучшена генерация - я упростил старую "слоёную" систему генерации. Если раньше она строилась по типу Зона1 => Соединение Зоны1 и Зоны2 => Зона 2 и так далее, то теперь я объединил зону и её соединительный слой что упростило понимание системы, но что более важно - это улучшило внешний вид генерации, поскольку теперь гарантированно будет генерироваться соединительный слой между зонами, в отличии от предыдущей версии где он мог не сгенерироваться из-за уже сгенерированных тайлов зоны.

Во-вторых, как и собирался, я улучшил алгоритм поиска ускорив его работу. Всё что для этого потребовалось - сделать его "пошаговым". Теперь алгоритм не перебирает все ветки решений по очереди, а как полагается волновому алгоритму - вначале находит все текущие ближайшие тайлы и только потом приступает к расчётам таких же тайлов для них. Благодаря этому удаётся заранее отбросить "тупиковые" ветки решений, которые в прошлой версии алгоритма приходилось пересчитывать целиком, начиная с того узла для которого находился менее длинный путь. Теперь же это происходит только для "особых" тайлов прямой путь до которых длиннее чем обходной что уже даёт огромный выигрыш в скорости работы алгоритма.

И последнее - я создал заготовку под будущий генератор объектов: https://github.com/Ylfgard/Practika1/tree/main/Assets/Scripts/MapSystem/ObjectsLayer
(3-й из 4-х запланированных генераторов карты). Он уже способен генерировать однотайловые объекты по типу деревьев, бочек и прочих объектов окружения, которые будут влиять на правила поиска пути: https://youtu.be/Yw08SPf9ZXQ
Но его планируется расширить, добавив ему возможность генерации стен (которые находятся между тайлами, а не на одном из них), а также разрушаемых объектов. Ну а также вместе с этим нужно будет расширить логику самих ячеек карты, так и логику поиска пути добавив различные фильтры и правила. Но это уже дело будущего меня.

### И так, самое время подвести итоги.

Первая практика со сроком в неделю подошла к концу и могу однозначно сказать - идея оправдала себя на все 200%. В рамках гейм джема или полноценной работы я бы навряд ли рискнул прикоснуться к насколько крупному проекту и точно не смог бы работать в настолько свободном формате.

Как и хотел я немного пощупал Zenject (хотя признаюсь хотелось бы больше, поскольку пока я толком даже не заглянул в его функционал) и сделал то что я так давно хотел - генератор карты и алгоритм поиска пути. Да и просто полученный опыт и знания от процесса работы тоже весьма и весьма ощутимые.

Так что резюмирую всё выше сказанное скажу, что эксперимент однозначно удачный! А это значит только одно - я буду проводить новые практики. Так что думаю, что не прощаюсь, поскольку я также буду выкладывать дневники по ним.

На сим всё! Спасибо за внимание. Я откланиваюсь и иду спать.
