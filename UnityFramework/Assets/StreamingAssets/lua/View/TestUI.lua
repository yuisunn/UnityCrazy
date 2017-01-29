local transform;
local gameObject;
local message;

TestUI = {}
this = TestUI

 
 function print_r(sth)
if type(sth) ~= "table" then
print(sth)
return
end

local space, deep = string.rep(' ', 4), 0
local function _dump(t)
local temp = {}
for k,v in pairs(t) do
local key = tostring(k)

if type(v) == "table" then
deep = deep + 2
print(string.format("%s[%s] => Table\n%s(",
string.rep(space, deep - 1),
key,
string.rep(space, deep)
)
) --print.
_dump(v)

print(string.format("%s)",string.rep(space, deep)))
deep = deep - 2
else
print(string.format("%s[%s] => %s",
string.rep(space, deep + 1),
key,
v
)
) --print.
end 
end 
end

print(string.format("Table\n("))
_dump(sth)
print(string.format(")"))
end
 
function TestUI.OKClick(obj)
	print("OKClick----------->>"..obj.name)
end
 

--启动事件--
function TestUI.Awake(obj) 
	gameObject = obj;
	transform = obj.transform;
	 print("---------------------------------------------------------------------------")

	this.btnOK = transform:FindChild('Button'):GetComponent('Button');  
	--this.btnOK.onClick = this.OKClick;
	this.image = transform:FindChild("Image"):GetComponent('Image');
	this.text = transform:FindChild("Text"):GetComponent('Text');

	this.text.text = "sdfsdfsdfsdfsdf"
	this.image.sprite = Resources.Load("Sprite/1"):GetComponent('SpriteRenderer').sprite
end
 


  