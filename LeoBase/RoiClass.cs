using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBase
{
    public class RoiClass
    {

    }

    public enum DrawMode
    {
        Rectangle1,
        Rectangle2,
        Circle,
        Ellispe,
        Region,
        Line,
        Point,
        MeasureRectangle2,
        MeasureCircle
    }

    public enum RoiOperation
    {
        New,//新建，会替换掉之前绘制ROI
        Union,//合并，与之前ROI合并成新的
        Difference,//得到之前ROI减去当前ROI的差集
        Intersection,//得到上一个ROI与当前ROI的交集
        Xor//得到上一ROI与当前ROI的对称差
    }

}
