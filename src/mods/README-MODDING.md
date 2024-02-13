# Server side modding

## Mod installations

Place mods as directory inside `src/mods`.
The mod directory must contain `manifest.xml` file in the first directory tree level.
For example: `src/mods/MyFirstMod/manifest.xml`

## `manifest.xml` syntax

Root node for `manifest.xml` is `<sodoffmod>`. It can contain child nodes:

* `<items>` - item database manipulation
* `<store>` - store database manipulation (not implemented yet)
* ...

### item database manipulation

`<items>` may contain may `<item>` child nodes. Each of them define one item modification. Modification type can be specified by `action` attribute, supported values:

* `add` - add new item
	* default when no `action` attribute
* `remove` - remove item
* `replace` - replace item definition for existed item id

`<item>` node may contains subnodes:

* `<id>` - specify item id
	* if not used item id will be read from item definition in `<data>`
* `<storeID>` - specify store id to add item to them
	* can occur multiple times
	* if not used item will be not added to any store
* `<data>` - item definition (syntax like `<I>` node in [src/Resources/items.xml](../Resources/items.xml))
	* can be committed in `remove` action`

#### Example

* remove Toothless ticket item (item id `8034`)
* add Night Furry Egg item (item id `29999`) and add it to store (store id `92`, for store description see comment in [src/Resources/store.xml](../Resources/store.xml).
* item id (for new items) should be unique to avoid mod collision, recommended format to use: `prefix * 10000 + private_id`, where:
	* `prefix` must be grater than 2 to avoid collision with original game and official SoDOff items 
	* `prefix` is unique mod author prefix (see SoDOff discord for details)
	* `private_id` is for digit number (0-9999) to free use by the this author
	* for example for `prefix = 789` and `private_id = 13` item id will be `7890013`

```
<sodoffmod>
	<items>
		<item action="remove">
			<id>8034</id>
		</item>
		<item>
			<storeID>92</storeID>
			<data>
				<an>RS_DATA/DragonEgg.unity3d/PfEggDevilishDervish</an>
				<at>
					<k>PetTypeID</k>
					<v>17</v>
					<id>29999</id>
				</at>
				<c>
					<cid>456</cid>
					<cn>Dragons Dragon Egg</cn>
					<i xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
					<id>29999</id>
				</c>
				<c>
					<cid>550</cid>
					<cn>Strike Class Eggs</cn>
					<i xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
					<id>29999</id>
				</c>
				<ct>0</ct>
				<ct2>250</ct2>
				<cp>0</cp>
				<d>Tame the cunning and mischievous Night Furry in your stables</d>
				<icn>RS_DATA/DragonEgg.unity3d/IcoEggDevilishDervish</icn>
				<im>-1</im>
				<id>29999</id>
				<itn>Night Furry Egg</itn>
				<l>false</l>
				<s>false</s>
				<as>false</as>
				<sf>10</sf>
				<u>-1</u>
				<rf>0</rf>
				<rtid>0</rtid>
			</data>
		</item>
	</items>
</sodoffmod>
```
