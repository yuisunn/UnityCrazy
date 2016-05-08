//using Microsoft.CSharp;
//using System;
//using System.CodeDom;
//using System.CodeDom.Compiler;
//using System.IO;
//using System.Reflection;
//class ActionCodemaker
//{ 

//    public void MakeActionList()
//    {
//        Type typ = typeof(Def.LogicActionDefine);
//        string[] str = Enum.GetNames(typ);
//        foreach (string s in str)
//        {  
//            MakeActionCode(s); 
//        }
//    }

//    public void MakeActionCode(String s)
//    {
//        string name = "Action" + s; 
//        string outputFile = "D:\\______Unity\\____SVN\\项目010\\code";//Environment.CurrentDirectory + "\\Assets\\Scripts\\Controller\\UnityAction";
//        string a = outputFile + "\\" + name + ".cs";
//        if (File.Exists(a))
//            return;

            
//        //准备一个代码编译器单元

//        CodeCompileUnit unit = new CodeCompileUnit();

//        //准备必要的命名空间（这个是指要生成的类的空间） 
//        CodeNamespace sampleNamespace = new CodeNamespace("");

//        //导入必要的命名空间 
//        sampleNamespace.Imports.Add(new CodeNamespaceImport("System"));
//        sampleNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
//        sampleNamespace.Imports.Add(new CodeNamespaceImport("System")); 

//        //准备要生成的类的定义

//        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(name);

//        //指定这是一个Class

//        Customerclass.IsClass = true;

//        Customerclass.TypeAttributes = TypeAttributes.Public;

//        //把这个类放在这个命名空间下

//        sampleNamespace.Types.Add(Customerclass);

//        //把该命名空间加入到编译器单元的命名空间集合中

//        Customerclass.BaseTypes.Add("LogicAction");

//        unit.Namespaces.Add(sampleNamespace);

//        //这是输出文件 

//        CodeMemberMethod moth = new CodeMemberMethod();
//        moth.Name = "ProcessAction";
//        moth.ReturnType = new CodeTypeReference("System.Boolean"); //返回值
//        moth.Attributes = MemberAttributes.Override | MemberAttributes.Public;//声明方法是公开 并且override的 
//        Customerclass.Members.Add(moth);

//        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

//        CodeGeneratorOptions options = new CodeGeneratorOptions();

//        options.BracingStyle = "C";

//        options.BlankLinesBetweenMembers = true;

//        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
//        {

//            provider.GenerateCodeFromCompileUnit(unit, sw, options);
//        }


//        //添加字段

//        //CodeMemberField field = new CodeMemberField(typeof(System.String), "_Id");

//        //field.Attributes = MemberAttributes.Private;

//        //Customerclass.Members.Add(field);

//        //添加属性

//        //CodeMemberProperty property = new CodeMemberProperty();

//        //property.Attributes = MemberAttributes.Public | MemberAttributes.Final;

//        //property.Name = "Id";

//        //property.HasGet = true;

//        //property.HasSet = true;

//        //property.Type = new CodeTypeReference(typeof(System.String));

//        //property.Comments.Add(new CodeCommentStatement("这是Id属性"));

//        //property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id")));

//        //property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Id"), new CodePropertySetValueReferenceExpression()));

//        //Customerclass.Members.Add(property);



//        //添加构造器(使用CodeConstructor) --此处略

//        //添加程序入口点（使用CodeEntryPointMethod） --此处略

//        //添加事件（使用CodeMemberEvent) --此处略

//        //添加特征(使用 CodeAttributeDeclaration)

//        //Customerclass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeAttributeDeclaration("Serializable"));

//        //生成代码


//    }
//}