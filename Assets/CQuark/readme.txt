﻿使用Wrap!

什么是Wrap?
Wrap是在编译前先把需要的类的各种方法和变量反射出来，一起编译。Wrap可以避免运行时反射造成的效率问题和GC问题。

Wrap是CQuark必须的吗？
不是。你如果不做Wrap，西瓜依然会使用反射（自动缓存）处理。但我依然推荐你把常用的类全部注册到Wrap中。

在哪些情况下使用？
1正式项目中
2执行效率测试时。

如何使用
点击菜单上面的CQuark/WrapMaker或者右击Project面板选择CQuark/WrapMaker
在Option中配置白名单和黑名单（白名单就是点击WrapCommon时会自动添加的类，黑名单是绝对不会添加进去的类）
你可以直接点击Option中的Reload来读取我事先写的配置
然后收起Option，点击WrapCommon，此时会注册白名单里所有的类和项目中没有命名空间的类（除非他在黑名单中）
如果需要注册其他的类，你可以在绿色框里输入类的全名或直接输入命名空间。点击WrapCustom
下面别的按钮用来更新或删除Wrap
