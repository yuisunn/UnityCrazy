local transform;
local gameObject; 


TestUI = {}
local this = TestUI
 
  
 
function TestUI.OKClick()
	print("OKClick----------->>")
end

function TestUI.ToggleClick()
end

function TestUI.InputEvent()
end

function TestUI.DropClick()
end

function TestUI.Slider()
end

function CreatePrefab(parent,prefab)
	local obj = GameObject.Instantiate(prefab)
	obj.name = "new Obj"
	obj.transform.parent = parent.transform;
end


--启动事件--
function TestUI.Awake(obj) 
	gameObject = obj;
	transform = obj.transform;
	print("---------------------------------------------------------------------------")

	this.btnOK = transform:FindChild('Button'):GetComponent('Button');  
	this.btnOK.onClick:AddListener(this.OKClick);
	this.image = transform:FindChild("Image"):GetComponent('Image');
	this.Text = transform:FindChild("Text"):GetComponent('Text');
	this.group = transform:FindChild("ToggleGroup"):GetComponent('ToggleGroup');
	this.toggle1 = this.group.transform:FindChild("Toggle1"):GetComponent('Toggle');

	this.Text.text = "sdfsdfsdfsdfsdf" 
end

function TestUI.Update(deltaTime,unscaledDeltaTime)  
	this.Text.text = "时间" .. deltaTime .. "时间2".. unscaledDeltaTime 
end
 


  