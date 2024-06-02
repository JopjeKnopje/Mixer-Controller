# Mixer-Controller
Controller for the Windows Volume Mixer
## The Idea
The idea behind this project is to have physical device to control the windows audio levels of certain applications.


### Features
- Individual app control
- Configurable with xml



### Todo
- [ ] Add button support with configurable action
- [ ] Add led support
- [ ] Add a display to the Controller to show what you are changing
- [ ] Add photos to repo
- [ ] Add build instructions
- [ ] Release a version
- [ ] Block diagram of workings



### Example config.xml
```xml
<settings>
	<comport>COM5</comport>
	<mixerchannel>
		<id>0</id>
		<prefix>fader</prefix>
		<application>discord</application>
	</mixerchannel>
	<mixerchannel>
		<id>1</id>
		<prefix>fader</prefix>
		<application>chrome</application>
	</mixerchannel>
	<mixerchannel>
		<id>2</id>
		<prefix>fader</prefix>
		<application>minecraft</application>
	</mixerchannel>
	<mixerchannel>
		<id>3</id>
		<prefix>fader</prefix>
		<application>spotify</application>
	</mixerchannel>
</settings>

```
