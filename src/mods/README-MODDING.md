# Server Side Modding

## Mod Installation

Place mods as individual directories inside `src/mods`.
Each mod folder must contain a `manifest.xml` file at its top directory level.
For example: `src/mods/MyFirstMod/manifest.xml`

## `manifest.xml` syntax

The root node in `manifest.xml` is `<sodoffmod>`. It can include the following child nodes:

* `<items>` - item database manipulation
* `<store>` - store database manipulation (not implemented yet)
* ...

### Item Database Manipulation

`<items>` may contain `<item>` child nodes. Each of them defines one item modification. The modification type can be specified by the `action` attribute. The supported values are:

* `add` - add new item
	* default if the `action` attribute is not present
* `remove` - remove item
* `replace` - replace item definition for an existed item ID

`<item>` may contain the following subnodes:

* `<id>` - item ID
	* if not used, the item ID will be retrieved from the item definition within `<data>`
* `<storeID>` - specifies the store ID to which the item will be added
	* can be used multiple times
	* if not used, the item will not be added to any store
* `<data>` - item definition (syntax like the `<I>` node in [src/Resources/items.xml](../Resources/items.xml))
	* can be omitted in the `remove` action

#### Example

* remove the Toothless ticket item (item ID `8034`)
* add a Night Furry Egg item (item ID `99999`) and add it to store (store ID `92`, for store description see comment in [src/Resources/store.xml](../Resources/store.xml))
* The item ID for new items should be unique to prevent mod collisions. The recommended format is: `prefix * 100000 + private_id`, where:
	* `prefix` must be grater than 0 to avoid collision with the original game and official SoDOff items
	* `prefix` is a unique mod author prefix
	* `private_id` ranges from 0 to 99999 and is available for unrestricted use by the author
	* for instance, if `prefix = 789` and `private_id = 123`, the item ID would be `78900123`

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
					<id>99999</id>
				</at>
				<c>
					<cid>456</cid>
					<cn>Dragons Dragon Egg</cn>
					<i xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
					<id>99999</id>
				</c>
				<c>
					<cid>550</cid>
					<cn>Strike Class Eggs</cn>
					<i xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true"/>
					<id>99999</id>
				</c>
				<ct>0</ct>
				<ct2>250</ct2>
				<cp>0</cp>
				<d>Tame the cunning and mischievous Night Furry in your stables</d>
				<icn>RS_DATA/DragonEgg.unity3d/IcoEggDevilishDervish</icn>
				<im>-1</im>
				<id>99999</id>
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
