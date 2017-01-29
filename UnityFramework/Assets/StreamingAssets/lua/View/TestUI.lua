local transform;
local gameObject; 


TestUI = {}
local this = TestUI
 
  
 
function TestUI.OKClick()
	this.slider.value = this.slider.value + 0.1;
	print("OKClick----------->>") 
end

function TestUI.ToggleClick(b)
	print("ToggleClick----------->>")
	this.input.text = "sdfsdf";
end

function TestUI.InputEvent()
	
end

function TestUI.DropClick(i)
	this.input.text =  i
end

function TestUI.Slider(f)
	this.input.text = f;
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
	this.group:SetAllTogglesOff();
	this.toggle1 = this.group.transform:FindChild("Toggle1"):GetComponent('Toggle');
	this.toggle1.onValueChanged:AddListener(this.ToggleClick)

	this.input = transform:FindChild('InputField'):GetComponent('InputField');
	
	this.dropdown = transform:FindChild('Dropdown'):GetComponent('Dropdown');
	this.dropdown.onValueChanged:AddListener(this.DropClick)

	this.slider = transform:FindChild('Slider'):GetComponent('Slider');
	this.slider.value = 0;
	this.slider.onValueChanged:AddListener(this.Slider)
end

function TestUI.Update(deltaTime,unscaledDeltaTime)  
	this.Text.text = "时间" .. deltaTime .. "时间2".. unscaledDeltaTime 
end
 


  