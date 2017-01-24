 

local transform;
local gameObject;
local TestPanel; 

TestPanel = {};
local this = TestPanel;

--启动事件--
function TestPanel.Awake(obj)
	gameObject = obj;
	transform = obj.transform;
	TestPanel = obj:GetComponent('TestPanel');
	print("sdfsdddddddddddddddddddddddddddddddddddddddd")

	this.InitPanel();
	logWarn("Awake lua--->>"..gameObject.name);
end

local funcOK = function() 
	this.ShowMsg('OK button clicked.'); 
end

local funcInvoke = function() 
	this.ShowMsg('Call button clicked, then call CSharp function.'); 
	TestPanel:Test();
end

local funcOpen = function() 
	PanelManager:OpenPanel("MessageBoxPanel", TestPanel.OnMessageBoxOpened);
end

function TestPanel.OnMessageBoxOpened()
	
end

--初始化面板--
function TestPanel.InitPanel()
	this.btnOK = transform:FindChild('Panel/LeftTop/btnOK').gameObject;
	this.btnInvoke = transform:FindChild('Panel/LeftTop/btnInvoke').gameObject;
	this.btnOpen = transform:FindChild('Panel/LeftTop/btnOpen').gameObject;

	local lblInfoObj = transform:FindChild('Panel/LeftTop/lblInfo').gameObject;
	this.lblInfo = lblInfoObj:GetComponent('UILabel');


	UIEventListener.Get(this.btnOK).onClick = funcOK;
	UIEventListener.Get(this.btnInvoke).onClick = funcInvoke;
	UIEventListener.Get(this.btnOpen).onClick = funcOpen;

end

function TestPanel.Start()
	logWarn("Start lua--->>"..gameObject.name);
end

this.startShowTime = 0;
function TestPanel.Update(deltatime, unscaledDeltaTime)
	if this.startShowTime > 0 and Time.time - this.startShowTime > 2 then
		this.lblInfo.text = '';
		this.startShowTime = 0;
	end

end

function TestPanel.ShowMsg(info)
	this.lblInfo.text = info;
	this.startShowTime = Time.time;
end

--单击事件--
function TestPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end