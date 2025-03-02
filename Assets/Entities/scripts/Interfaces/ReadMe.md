# Entity interfaces
**интерфейсы, которые содеражат поведение ентити**

## IEntity:
**Базовый интерфейс для всех остальных**
### Свойства:
- Entity EntityReference: ссылка на ентити, который реализует этот интерфейс

## IAttacker:
**Интерфейс, показывающий, что ентити может атаковать других**
### Свойства:
- IHealthable Target: цель, которую атакует ентити
### Ивенты:
- UnityEvent<IHealthable> OnSetTargetEvent: инвет, который вызывается, когда меняется свойство Target
### Методы:
- void SetTarget(IHealthable target = null): задаёт значение свойтсву Target

## IControllable:
**Интерфейс-флаг, чтобы понимать может ли игрок управлять данным ентити**

## IDropable:
**Интерфейс, показывающий, что ентити может выдать предметы**
### Свойства:
- IEnumerable<ResourceCountPair> DropableItems: предметы, которые выдаёт ентити
### Методы:
- void Drop(IInventoriable inventory): выдаёт предметы ентити(inventory), который реализовать интерфейс IInventoriable

## IHealthable:
**Интерфейс, показываюший, что ентити имеет здоровье, может умереть**
### Свойства:
- EntityHealth HealthComponent: 
- float Health: текущее значение здоровья
- float Regeneration: значение регенерации
- bool Alive: жив ли ентити
### Методы:
- void Damage(float damage, Entity by = null): уменьшает здоровье 
- void Heal(float damage, Entity by = null):  увеличивает здоровье

## IIconable:
**Интерфейс, показывающий, что ентити имеет визуальное отображение для HUD'ов
### Свойства:
- Sprite Icon: иконка ентити

## IIntaractable:
**Интерфейс, показываюший, что с ентити можно взаимодействовать**
### Свойства:
- bool Interactable: можно ли взаимодействовать с ентити
### Ивенты:
- UnityEvent<IInteractor> OnInteracEvent: событие, которое вызывает при взаимодейтсвии с объектом 
### Методы: 
- void InteractBy(IInteractor entity): вызывает ивент OnInteracEvent, в который передаёт entity, если entity не null

## IInteractor:
**Интерфейс, показывающий, что ентити может взаимодействовать с IIntaractable**
### Свойства:
- InteractingBehaviour InteractorComponent: компонент, отвечающий за взаимодействие 

## IInventoiable:
**Интерфейс, показывающий, что ентити может хранить предметы**
### 
###
###

## IMovable:

## IRestorable:

## IStatsable:

## ITaskSolver:
