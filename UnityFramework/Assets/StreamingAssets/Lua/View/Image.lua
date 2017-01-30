local transform;
local gameObject; 


Image = {}
local this = Image
 
  
 
function Image.OKClick()
	TestUI.input.text = "itemclick";
	print("OKClick----------->>")
end
 

--Æô¶¯ÊÂ¼þ--
function Image.Awake(obj) 
	gameObject = obj;
	transform = obj.transform;
	print("---------------------------------------------------------------------------")

	this.btnOK = gameObject:GetComponent('Button');  
	this.btnOK.onClick:AddListener(this.OKClick);
	 
	  
end 
 


  