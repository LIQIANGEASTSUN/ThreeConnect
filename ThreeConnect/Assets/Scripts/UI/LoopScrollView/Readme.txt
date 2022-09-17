

无限滚动 ScrollView 使用

在 Content 上挂 LoopScrollView 脚本
Content 路径为 ScrollView/Viewport/Content


然后设置参数值 
Left：子物体距离左侧距离

Top：子物体距离上方距离

CellSize：子物体尺寸

Spacing：子物体间隔

FixedRowCol: 固定行/列，竖直滑动选择固定列，水平滑动选择固定行

FixedCount：每行/列固定的个数，最小为 1


代码调用如下
// 获取 LoopScrollView 组件
Transform contentTr = transform.Find("ScrollView/Viewport/Content")
LoopScrollView  _loopScrollView = contentTr .GetComponent<LoopScrollView>();

// 初始化设置每一项刷新的函数，每一个子物体UI组件的赋值在 RefreshItem 回调函数中
_loopScrollView.Init(RefreshItem);

// 设置子物体总数
_loopScrollView.ItemCount(130);


// 第几个子物体跳转到最左侧或者最上方
int goToIndex = 10;
_loopScrollView.GoToIndex(goToIndex);


///  Cell 子项显示类型
/// 在 ScrollView 显示区域的 上方、下方、左边、右边、中间(当前显示)
CellShowType cellShowType = _loopScrollView.GetCellShowType(50);


