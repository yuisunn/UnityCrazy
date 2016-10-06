Shader "ShaderNote/ShaderTest"
{
	Properties //属性
	{
		//和 subshader里定义的 资源名同名才可以对接上
		_MainTex("Texture", 2D) = "white" {}//声明贴图属性
		_Color("Color Tint", Color) = (1,1,1,1); 
		_Gloss("Gloss", Range(8.0, 256)) = 20;
	}
		//可以有多个 subshader
	SubShader //显卡a用
	{ 
		CGINCLUDE
		//类似 c++ 头文件
		ENDCG

		Tags{ 
			"RenderType" = "Opaque" 

			/*
			Background 1000 在任何其他队列之前被渲染
			Geometry 2000  默认渲染队列 不透明物体都使用这个
			AlphaTest  2450 5.0新加 透明物体队列
			Transparent 3000 使用了透明度混合的用 在geometry 和 alphatest 后渲染 在按从后往前渲染
			Overlay 4000 实现叠加效果  最后渲染的物体
			*/
			//"LightMode" = "ForwardBase"
			//			  =	"ShadowCast"
			//"DisableBatching" = "True" 批处理
		}//选raw你队列和透明相关
		Pass
		{

		
		//如果硬件不支持 当前渲染路径 会使用低级路径 这时候会根据 LightMode 找到对应pass
		Tags{ "LightMode" = "ForwardBase" }
		/*
		ForwardBase 向前渲染的光照等  渲染设置要写  默认pass是顶点逐顶点光照
			#pragma   否则光照变量 会不正确 
			可以访问光照贴图 lightmap
		ForwardAdd 计算额外的逐像素光源 一个pass 对应1个光源 
			#pragma multi_compile_fwdadd
			#pragma multi_compile_fwdadd_fullshadows 需要用开启阴影
			通常混合模式用 blend one one
		Deferred 会渲染g缓冲
		ShadowCaster 计算阴影然后映射到阴影贴图
		PrepassBase  延迟光照的 法线和高光反射
		PrepassFinal 延迟光照最后一布 混合 纹理 和 光照 输出最后图像
		Always 不管什么路径 pass 总是被渲染 但不计算任何光照
		*/

		/*
		unity 处理光照方式 
		1每个顶点 2每个像素 3球谐函数
		根据渲染模式决定用那个
		light组件-》rendermode important 逐像素光源 no important 逐顶点 
		unity会根据实际情况 （距离光源强度等）选择用哪个光源
		*/
			/*
			ZWrite Off 关闭深度缓存
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask 0 //pass不会输出任何颜色 
			Blend SrcAlpha OneMinusSrcAlpha, One Zero 可以一次使用多个混合命令
			//设置哪些物体背面剔除
			Cull Back | Front | off 渲染物体的正面背面 用两个pass 分别渲染正面和背面
			

			*/
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members worldNormal)
#pragma exclude_renderers d3d11 xbox360
			//包含头文件
			#include "UnityCG.cginc"//常用函数  宏 结构
			//unityshaervariables 编译unityshader时会自动包含进去 包含了内置全局变了 UNITY_MATRIX_MVP等
			//Lighting 包含各种内置光照模型
			//HLSLSupport 自动包含进去 声明了很多跨平台编译的宏和定义
			//UnityCG包含常用结构体
			//appdata_base  appdata_tan appdata_full appdata_img v2f_img
			//常用函数
			//WorldSpaceViewDirf 返回值姐空间中顶点到view相机的距离向量
			//...
			//UnityObjectToWorldNormal 吧发现重模型空间编导世界空间
			//UnityWorldSpaceLightDir 输入一个模型空间中的顶点位置 返回世界空间中从该点到光源的光照方向 只能用于向前渲染
			//。。。
			//TRANSFORM_TEX 宏 = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

			//UNITY_LIGHTMODEL_AMBIENT //unity环境光颜色强度信息

			//SHADOW_COORDS(2) 声明一个阴影贴图采样坐标
			//	UNITY_NO_SCREENSPACE_SHADOW 判断是否支持屏幕空间阴影 
			//TRANSFER_SHADOW(0)获得阴影贴图采样坐标
			//	ComputeScreenPos 计算坐标 把顶点从模型空间转换到光源空间
			//SHADOW_ATTENUATION 计算着色器阴影
		    //	关闭阴影 SHADOW_ATTENUATION =1
			//自定义变量名要和宏中变量同名 否则用不了
			//UNITY_LIGHT_ATTENUATION（attend（unity自动生成的变量），i（v2f结构），i。worldpos） 光照衰减 宏
			// unity会对不同平台调用不同的UNITY_LIGHT_ATTENUATION重载
			#pragma target 2.0 //2.0-5.0 shader model 版本
		    
			#pragma vertex vert //包含vertex 定义vert函数为
			#pragma fragment frag //包含fragment 定义frag函数为
/*
#pragma multi_compile_shadowcaster
*/
//xxxx_TexelSize 是unity 提供访问xx纹理的纹素大小 512*512 纹素1/512
uniform half4 _MainTex_TexelSize;
			
			float4 vert(float4 v: POSITION) : SV_POSITIION //SV_POSITIION 是返回参数类型 必须带SV_POSITIION
			{
				return mul(UNITY_MATRIX_MVP,v);
			}
			fixed4 frags() : SV_Target //SV_POSITIION 是返回参数类型 必须带SV_POSITIION
			{
				return fixed4(1.0,1.0,1.0,1.0);
			}

			struct a2v
			{
			//cg里的语义 定义一个参数的含义 哪里读取什么格式读取 输出到哪 输出什么格式
			//NORMAL 分量范围在 [-1.0,1.0] 没有在unity里把texture改成 normalmap(unity可以在不同平台下做压缩优化)就得手动计算 从像素的[0,1]范围变到 -1-1 
			/*
					UnpackNormal 法线解压宏 需要选为 normalmap才能用
					normal.xy = packednormal.wy * 2 - 1;
					normal.z = sqrt(1 - normal.x*normal.x - normal.y * normal.y);
					return normal;
				高度图除了选为 normalmap 还要勾上create from grayscale 
					计算凹凸程度 1使用sobel滤波 2Smooth
			*/
			//POSITION float3
			//Tangent 切线float4
			//TEXCOORD 1。2.。。是代表第几组
			//COLOR  fixed4 float4
			//TYPE类型 name名字: semantic定义;
			//ShaderLab属性类型  
			//float4,half4,fixed4
			//float 最高精度浮点 通常用32位存储,
			//half 通常用16位存储  精度在 -60000-60000
			//fixed通常用11为存储  -2.0-2.0  //旧移动平台会用
		    //大多数桌面gpu他们三个的都会按最高精度浮点32位
			//移动平台就不不同了
			float4 vertex : POSITION;
			float3 normal : NORMAL; 
			float4 texcoord : TEXCOORD0; 
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 worldNormal： TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};

			float4 vert(a2v v) : SV_POSITION
			{
			   v2f o;
			   o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); //unity 一般是矩阵乘以列向量(?) 
			   o.worldNormal = UnityObjectToWorldNormal(v.normal);
			   o.worldPos = mul(w_matrix, v.vertex).xyz;
			   o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;// 先对纹理属性 xy 用_MainTex_STxy缩放 在用zw做偏移
			   //o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);//也可以
			   return o;
			}

			fixed4 frag() : SV_Target
			{
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

				fixed3 albedo = tex2D(_mainTex, i.uv).rgb * _Color.rgb;
				fixed3 diffuse = _LIghtColor0.rgb * albedo * max(0, dot(worldNormal, WorldLightDir));

			}
			ENDCG
		}
	}
	//SubShader //显卡b用
	//{

	//}
	//unity会在 寻找一个阴影pass 如果没有就用unity自带的 Fallback包含这样一个pass
	fallback "VertexLits" //也可以用off 关闭 
}



//unity不同平台差异
/*
//1 opengl屏幕坐标 坐下 00  dx 左上 00
//unity会自动dx平台下 反转图像纹理 达到跨平台 只有一种情况下不会自动就是开启抗锯齿
//这种情况 unity 先渲染屏幕图像 在硬件抗锯齿
//Graphics.Blit 函数已经对纹理坐标进行了处理 同时处理 多张图 纹理坐标就可能不同

//2#if UNITY_UI_STARTS_AT_TOP //当前平台是否是dx
//		if (Tex_TexlSize.y < 0)
//		uv.y = 1 - uv.y;

//3 判断 aphla是否透明
//				clip (texColor.a - _Cutoff);
// 
//				if ((texColor.a - _Cutoff) < 0.0) {
//					discard;
//				}s
//#endif

2 float4 = float4(0.0) dx11 不支持必须写全4个参数

3 用UNITY__INITIALIZE__OUTPUT 初始化结构可以

4 dx不支持在顶点着色其用tex2d 可以用 tex2dlod代替  需要添加#pragma target3.0 需要shader model3.0支持
*/

/* shader注意
1不用循环 和
2控制 if else 语句 用的话条件变量是常数
3不要除以0
 
*/

/*
切线空间的法线在边缘会由缝隙 和可见缝合 
可进行uv动画 
可以重用法线问里z防线重视正的 法线可压缩 切线空间下的法线的z总是正的 可以只存储xy
*/

/*切线空间
1：切空间是为了转换法线贴图到模型表面。模型表面首先有一个法线方向，然后有一个切方向，就能定下来一套模型表面的转换矩阵。法线贴图上面的表面法线偏移量乘以这个转换矩阵，才能转换到模型表面对应的方向。然后再用这个偏移量计算渲染时候用的法线。
2：法线贴图上存储的是表面法线的偏移。 
*/

/*
unity render path
forward render
每个 光源都走pass 计算一边光照 
unity4.x内置使用切线空间计算光照 5.x用的世界空间
延迟光照 
unity有两种延迟路径 4.x 和5.x +
渲染纹理
RT0 argb32 漫反射
RT1 ...高光
RT2 argb2101010 法线
RT3 argb32 hdr 自发光


*/

/*
shader 寄存器
输入寄存器从顶点缓冲中读取数据
常量寄存器提供ALU计算需要用到的各种常量	
	常量寄存器在vertex shader开始执行指定程序之前被CPU加载。常量寄存器是只读的， 一般用于储存例如光源位置、材质、特殊动画所需数据等参数
	常量寄存器可以通过地址寄存器a0.x来间接寻址
	如果一条指令需要引用超过一个的常量寄存器，它只能通过暂存寄存器来引用。
临时寄存器存储shader计算用到的临时变量
输出寄存器存放shader程序计算的结果
	输出寄存器在光栅化时可以被使用
插值寄存器 存放军阵变换
Vertex Shader中的所有数据都被表现为128-bit的四元数 dx8.1一个Vertex shader使用16个输入寄存器
*/

/*
budong qiexian  toushijuzhen  shitongkongjian  dao pingmukongjian 
切线空间 法线转换
法线空间 = （法线乘以切线）*切线w分量
*/

/*
渐变纹理
 通过 halfLambert 计算的到 一个uv 贴图坐标
 到渐变贴图中找到对应像素

遮罩纹理

*/

/*
内置光照变量
_LightColor() 该pss的逐像素光源颜色
_WorldSpaceLIghtPos() 逐像素光源位置 平行光 w=0 其他 w=1
_LightMatrix()世界空间到光源空间的变换矩阵
unity_4LightAtten() 点光源衰减参数
unity_LightColor() 光源颜色
unity_4LightPosX0,...点光源世界空间位置
*/

/*
unity光源  
edit->projectsetting quality-pixedl light count 控制最多可以接受的光源数量  最亮的
这几个光源会被 forwadadd pass 计算
面光源 area light

阴影 
mesh 的属性
	Shadowcast 把物体加入到光源阴影贴图
	receive shadows 是否接受来自其他物体的阴影
	castShadows two sided对所有面计算阴影

*/

/*
unity优化 
edit->projectsetting quality-pixedl light count 控制前置渲染光源数量
*/

/*
立方体纹理 在透明渲染后被渲染
	不能反射自身 尽量使用凸面体 凹面体会反射自身
	wrapmode用clamp 防止接缝不匹配
	windows->light->skybox 用于所有相机 tint color 用来控制整体颜色 exposure 亮度 rotation z轴方向
		如果要用单独的需要对相机单独设置skybox
	//unity后根据相机自动生成cubemap的方法
*/

/*
幕后效果
MonoBehaviour>OnRenderImage()获得渲染后的图像
	正常是在透明不透名都渲染完调用
	也可以通过加 [ImageEffectOpaque] 属性实现在哪个环节调用
Graphics.Blit（src， dest没mat ，pass（默认-1依次调用，否则调用指定索引的））完成处理
*/


/*
世界矩阵
视图矩阵
透视矩阵
非线性矩阵 z轴非线性 就是 一阶导数不是常数 
离得越近 z的取值乏味越大 越远 越小
*/

/*
深度纹理 深度和法线缓存
类型为opaque队列 渲染队列到校小于等于 2500的 就渲染到深度和法线纹理 是一个单独pass获得
深度纹理 精度24或16 向前渲染默认是不会创建法线缓存的
ps3 psp上 不能用 tex2D 直接获得 unity 封装了宏统一宏 SAMPLE_DEPTH_TEXTURE 用来处理平台问题
HLSLSupport。cginc 中定义了
mvp后深度非线性到线性 用 LinearEyeDepth 转换到视角空间下的深度值 LinearEyeDepth 0-1
tex2D(_CamerDepthNormalsTexture) 获得法线和深度
DecodeDepthNormal 解压缩法线和深度
*/

/*
性能分析工具
android 高通adreno  英伟达nvperfhud
ios powervram xcode  powervr芯片基于瓦片的延迟渲染器 跟踪不到 draw call

动态批处理 
1能进行动态批处理的顶点属性规模不要小于900 规模= 顶点数量x顶点位置法线等顶点属性
25.0 使用相同缩放尺寸限制不在了
3光照纹理 lightmap  需要指向光照纹理同一位置
4多pass shader会中断批处理

静态批处理 子集关系 都变到世界空间—》合并到同一网格-》相同材质的—》一个批次渲染
在开始之前把需要的模型合并到一个网格结构中 不模型都转换到世界空间 然后调用一个批处理
1不可以在运行阶段被移动
2占用更多内存 静态批处理中一些物体共享了相同的模型 在内存中每个每个物体对应一个该网格的复制品


脚本中访问共享材质 用 Renderer。SharedMaterial renderer。material 用来修改该材质但会生成一个复制品

优化 
批处理
	尽可能静态批处理 小心内存消耗
	不得不用动态批处理要考虑各种限制
	1小道具可以用动态
	2动画中不动的可以用静态
	透明物体unity 优先保证渲染顺序在批处理
几何
	gpu 和 建模软件 顶点数不一样原因 
		要分离纹理坐标（虽然顶点一样 但和这个顶点相连的多个面纹理采样坐标是不一样的 gpu理解不聊 ） 
		产生平滑边界 （这一顶点有多个法线和纹理信息）
	1移除不必要的硬边和纹理链接
	2避免边界平滑和纹理分离
	3用lod
	4遮挡剔除
	5遮挡剔除可以去除被挡住的物体（视锥只会剔除不在摄像机范围内的物体 ）
减少处理的片原
	overdraw绘制顺序 不要重复渲染（从前往后渲染后面的物体无法通过深度测试不会渲染） 
	background geometry aplphatest opaque 渲染队列小于2500 的对象不透明是从前往后绘制
	transparent overlay 半透明物体 没有深度测试 只能从后往前绘制
	也就是被经常被遮挡的物体 用从前往后渲染 先渲染 然后有了最前面的物体深度信息 后面的就不会渲染了
	半透明物体 不能避免overdraw
	1经历减少面积
	2gui使用透明度混合 比 透明度测试要好
光照 减少时实光照计算
	1用lighthmap 光照烘培
	2god ray
	3lookup texture lut查表 取光照信息
节省带宽
	1纹理长宽值是2的幂
	2多用mipmapping
	3调整分辨率 Screen.SetResolution
	4纹理压缩
减少计算复杂度
	1使用shader的 lod 来指定sahder复杂度
		使用Shader.maximumLod 或 Shader.globalMaximumLod 来设置允许的最大lod值 大于的lodshader不会运行
	2代码
		1对象《顶点《像素 越往后复杂度越高
			尽量在前面就解决问题
		2使用低精度的变量  half的速度是float的两倍
			fixed lowp 用于颜色 和归一化的向量 他是float的4背
			
		3避免不同精度转化 swizzle操作
		4屏幕特效经量少用 合并到一个pss
		5高精度的可以用lut 或一道顶点着色器处理
		5不要用分支语句 if for等
		6避免sin tan等复杂运算操作
		7经可能不要用discard忽略渲染顺序
	
*/