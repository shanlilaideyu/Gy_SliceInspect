using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBase
{
    [Serializable]
    public class HsbObj
    {
        public HsbObj()
        {
            ObjLock = new object();
        }
        private HObject obj;
        public object ObjLock;
        public virtual HObject Value
        {
            set
            {
                lock (ObjLock)
                {
                    if ((obj == null || !obj.IsInitialized()))
                        HOperatorSet.GenEmptyObj(out obj);
                    if (value != null && value.IsInitialized())
                    {
                        if (!HObject.ReferenceEquals(value, obj) && value.TestEqualObj(obj) == 0)
                        {
                            obj.Dispose();
                            obj = value.CopyObj(1, value.CountObj());
                        }
                    }
                    else
                    {
                        if (obj != null && obj.IsInitialized())
                            obj.Dispose();
                        HOperatorSet.GenEmptyObj(out obj);
                    }
                }
            }
            get
            {
                lock (ObjLock)
                {
                    if (!(obj != null && obj.IsInitialized()))
                        HOperatorSet.GenEmptyObj(out obj);
                }
                return obj;
            }
        }

        public bool Exist
        {
            get
            {
                lock (this.ObjLock)
                {
                    return Value.CountObj() > 0;
                }
            }
        }
        public void Dispose()
        {
            lock (this.ObjLock)
            {
                Value.Dispose();
            }
        }
    }
    public class HsbRegion : HsbObj
    {
        private HsbColor color;

        public HsbColor Color
        {
            get { return color; }
            set { color = value; }
        }
        private int colorNum = 1;

        public int ColorNum
        {
            get { return colorNum; }
            set { colorNum = value; }
        }
        private HsbDraw draw;

        public HsbDraw Draw
        {
            get { return draw; }
            set { draw = value; }
        }

    }

    public class HsbRegions : List<HsbRegion>
    {
        new public void Remove(HsbRegion Region)
        {
            this.Remove(Region);
            Region.Dispose();
        }
        public void Dispose()
        {
            foreach (var item in this)
            {
                item.Dispose();
            }
            this.Clear();
        }
    }

    public enum HsbColor
    {
        red,
        green,
        gray,
        blue,
        yellow,
        pink,
        magenta
    }
    public enum HsbDraw
    {
        margin,
        fill
    }
    public enum HsbRegionOperate
    {
        union,
        difference
    }
}
