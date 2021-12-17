// Decompiled with JetBrains decompiler
// Type: akControl.clsCommon
// Assembly: akControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F92C7728-1D4E-494E-B969-052940ECF948
// Assembly location: D:\bomming\CYG610-V1.0\CygCAP0410（7）(ou)411now\CygCAP0411now\CygCAP\bin\Debug\akControl.dll

using HalconDotNet;

namespace akControl
{
  internal class clsCommon
  {
    public static void disp_message(
      HTuple hv_WindowHandle,
      HTuple hv_String,
      HTuple hv_CoordSystem,
      HTuple hv_Row,
      HTuple hv_Column,
      HTuple hv_Color,
      HTuple hv_Box)
    {
      HTuple red = (HTuple) null;
      HTuple green = (HTuple) null;
      HTuple blue = (HTuple) null;
      HTuple row1_1 = (HTuple) null;
      HTuple column1 = (HTuple) null;
      HTuple row2_1 = (HTuple) null;
      HTuple column2_1 = (HTuple) null;
      HTuple row1 = (HTuple) null;
      HTuple column = (HTuple) null;
      HTuple width1 = (HTuple) null;
      HTuple height1 = (HTuple) null;
      HTuple maxAscent = (HTuple) null;
      HTuple maxDescent = (HTuple) null;
      HTuple maxWidth = (HTuple) null;
      HTuple maxHeight = (HTuple) null;
      HTuple htuple1 = new HTuple();
      HTuple htuple2 = new HTuple();
      HTuple htuple3 = new HTuple();
      HTuple htuple4 = new HTuple();
      HTuple exception = new HTuple();
      HTuple htuple5 = new HTuple();
      HTuple htuple6 = new HTuple();
      HTuple ascent = new HTuple();
      HTuple descent = new HTuple();
      HTuple width2 = new HTuple();
      HTuple height2 = new HTuple();
      HTuple htuple7 = new HTuple();
      HTuple htuple8 = new HTuple();
      HTuple htuple9 = new HTuple();
      HTuple htuple10 = new HTuple();
      HTuple mode = new HTuple();
      HTuple htuple11 = new HTuple();
      HTuple htuple12 = hv_Box.Clone();
      HTuple htuple13 = hv_Color.Clone();
      HTuple htuple14 = hv_Column.Clone();
      HTuple htuple15 = hv_Row.Clone();
      HTuple htuple16 = hv_String.Clone();
      HOperatorSet.GetRgb(hv_WindowHandle, out red, out green, out blue);
      HOperatorSet.GetPart(hv_WindowHandle, out row1_1, out column1, out row2_1, out column2_1);
      HOperatorSet.GetWindowExtents(hv_WindowHandle, out row1, out column, out width1, out height1);
      HOperatorSet.SetPart(hv_WindowHandle, (HTuple) 0, (HTuple) 0, height1 - 1, width1 - 1);
      if ((int) new HTuple(htuple15.TupleEqual( -1)) != 0)
        htuple15 = (HTuple) 12;
      if ((int) new HTuple(htuple14.TupleEqual( -1)) != 0)
        htuple14 = (HTuple) 12;
      if ((int) new HTuple(htuple13.TupleEqual(new HTuple())) != 0)
        htuple13 = (HTuple) "";
      HTuple htuple17 = ((HTuple) "" + htuple16 + "").TupleSplit((HTuple) "\n");
      HOperatorSet.GetFontExtents(hv_WindowHandle, out maxAscent, out maxDescent, out maxWidth, out maxHeight);
      HTuple row1_2;
      HTuple htuple18;
      if ((int) new HTuple(hv_CoordSystem.TupleEqual((HTuple) "window")) != 0)
      {
        row1_2 = htuple15.Clone();
        htuple18 = htuple14.Clone();
      }
      else
      {
        HTuple htuple19 = (HTuple) 1.0 * height1 / (row2_1 - row1_1 + 1);
        HTuple htuple20 = (HTuple) 1.0 * width1 / (column2_1 - column1 + 1);
        row1_2 = (htuple15 - row1_1 + 0.5) * htuple19;
        htuple18 = (htuple14 - column1 + 0.5) * htuple20;
      }
      HTuple htuple21 = (HTuple) 1;
      HTuple color1 = (HTuple) "gray";
      if ((int) new HTuple(htuple12.TupleSelect((HTuple) 0).TupleEqual((HTuple) "true")) != 0)
      {
        if (htuple12 == null)
          htuple12 = new HTuple();
        htuple12[0] = (HTupleElements) "#fce9d4";
        color1 = (HTuple) "#f28d26";
      }
      if ((int) new HTuple(new HTuple(htuple12.TupleLength()).TupleGreater((HTuple) 1)) != 0 && (int) new HTuple(htuple12.TupleSelect((HTuple) 1).TupleEqual((HTuple) "true")) == 0)
      {
        if ((int) new HTuple(htuple12.TupleSelect((HTuple) 1).TupleEqual((HTuple) "false")) != 0)
        {
          htuple21 = (HTuple) 0;
        }
        else
        {
          color1 = (HTuple) htuple12[1];
          try
          {
            HOperatorSet.SetColor(hv_WindowHandle, htuple12.TupleSelect((HTuple) 1));
          }
          catch (HalconException ex)
          {
            ex.ToHTuple(out exception);
            throw new HalconException((HTuple) "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)");
          }
        }
      }
      if ((int) new HTuple(htuple12.TupleSelect((HTuple) 0).TupleNotEqual((HTuple) "false")) != 0)
      {
        try
        {
          HOperatorSet.SetColor(hv_WindowHandle, htuple12.TupleSelect((HTuple) 0));
        }
        catch (HalconException ex)
        {
          ex.ToHTuple(out exception);
          throw new HalconException((HTuple) "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)");
        }
        htuple17 = (HTuple) " " + htuple17 + " ";
        HTuple t2 = new HTuple();
        for (HTuple index = (HTuple) 0; (int) index <= (int) (new HTuple(htuple17.TupleLength()) - 1); index = (HTuple) ((int) index + 1))
        {
          HOperatorSet.GetStringExtents(hv_WindowHandle, htuple17.TupleSelect(index), out ascent, out descent, out width2, out height2);
          t2 = t2.TupleConcat(width2);
        }
        HTuple htuple19 = maxHeight * new HTuple(htuple17.TupleLength());
        HTuple htuple20 = new HTuple(0).TupleConcat(t2).TupleMax();
        HTuple row2_2 = row1_2 + htuple19;
        HTuple column2_2 = htuple18 + htuple20;
        HOperatorSet.GetDraw(hv_WindowHandle, out mode);
        HOperatorSet.SetDraw(hv_WindowHandle, (HTuple) "fill");
        HOperatorSet.SetColor(hv_WindowHandle, color1);
        if ((int) htuple21 != 0)
          HOperatorSet.DispRectangle1(hv_WindowHandle, row1_2 + 1, htuple18 + 1, row2_2 + 1, column2_2 + 1);
        HOperatorSet.SetColor(hv_WindowHandle, htuple12.TupleSelect((HTuple) 0));
        HOperatorSet.DispRectangle1(hv_WindowHandle, row1_2, htuple18, row2_2, column2_2);
        HOperatorSet.SetDraw(hv_WindowHandle, mode);
      }
      for (HTuple index = (HTuple) 0; (int) index <= (int) (new HTuple(htuple17.TupleLength()) - 1); index = (HTuple) ((int) index + 1))
      {
        HTuple color2 = htuple13.TupleSelect(index % new HTuple(htuple13.TupleLength()));
        if ((int) new HTuple(color2.TupleNotEqual((HTuple) "")).TupleAnd(new HTuple(color2.TupleNotEqual((HTuple) "auto"))) != 0)
          HOperatorSet.SetColor(hv_WindowHandle, color2);
        else
          HOperatorSet.SetRgb(hv_WindowHandle, red, green, blue);
        HTuple row2 = row1_2 + maxHeight * index;
        HOperatorSet.SetTposition(hv_WindowHandle, row2, htuple18);
        HOperatorSet.WriteString(hv_WindowHandle, htuple17.TupleSelect(index));
      }
      HOperatorSet.SetRgb(hv_WindowHandle, red, green, blue);
      HOperatorSet.SetPart(hv_WindowHandle, row1_1, column1, row2_1, column2_1);
    }

    public static void set_display_font(
      HTuple hv_WindowHandle,
      HTuple hv_Size,
      HTuple hv_Font,
      HTuple hv_Bold,
      HTuple hv_Slant)
    {
      HTuple information = (HTuple) null;
      HTuple windowHandle = new HTuple();
      HTuple ascent = new HTuple();
      HTuple descent = new HTuple();
      HTuple width = new HTuple();
      HTuple height = new HTuple();
      HTuple htuple1 = new HTuple();
      HTuple exception = new HTuple();
      HTuple htuple2 = new HTuple();
      HTuple font1 = new HTuple();
      HTuple font2 = new HTuple();
      HTuple htuple3 = new HTuple();
      HTuple htuple4 = new HTuple();
      HTuple indices1 = new HTuple();
      HTuple htuple5 = new HTuple();
      HTuple htuple6 = new HTuple();
      HTuple indices2 = new HTuple();
      HTuple htuple7 = new HTuple();
      HTuple htuple8 = new HTuple();
      HTuple htuple9 = hv_Bold.Clone();
      HTuple t2 = hv_Font.Clone();
      HTuple htuple10 = hv_Size.Clone();
      HTuple htuple11 = hv_Slant.Clone();
      HOperatorSet.GetSystem((HTuple) "operating_system", out information);
      if ((int) new HTuple(htuple10.TupleEqual(new HTuple())).TupleOr(new HTuple(htuple10.TupleEqual( -1))) != 0)
        htuple10 = (HTuple) 16;
      if ((int) new HTuple(information.TupleSubstr((HTuple) 0, (HTuple) 2).TupleEqual((HTuple) "Win")) != 0)
      {
        try
        {
          HOperatorSet.OpenWindow((HTuple) 0, (HTuple) 0, (HTuple) 256, (HTuple) 256, (HTuple) 0, (HTuple) "buffer", (HTuple) "", out windowHandle);
          HOperatorSet.SetFont(windowHandle, (HTuple) "-Consolas-16-*-0-*-*-1-");
          HOperatorSet.GetStringExtents(windowHandle, (HTuple) "test_string", out ascent, out descent, out width, out height);
          HTuple htuple12 = (HTuple) 110.0 / width;
          htuple10 = (htuple10 * htuple12).TupleInt();
          HOperatorSet.CloseWindow(windowHandle);
        }
        catch (HalconException ex)
        {
          ex.ToHTuple(out exception);
        }
        if ((int) new HTuple(t2.TupleEqual((HTuple) "Courier")).TupleOr(new HTuple(t2.TupleEqual((HTuple) "courier"))) != 0)
          t2 = (HTuple) "Courier New";
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "mono")) != 0)
          t2 = (HTuple) "Consolas";
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "sans")) != 0)
          t2 = (HTuple) "Arial";
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "serif")) != 0)
          t2 = (HTuple) "Times New Roman";
        HTuple htuple13;
        if ((int) new HTuple(htuple9.TupleEqual((HTuple) "true")) != 0)
        {
          htuple13 = (HTuple) 1;
        }
        else
        {
          if ((int) new HTuple(htuple9.TupleEqual((HTuple) "false")) == 0)
            throw new HalconException((HTuple) "Wrong value of control parameter Bold");
          htuple13 = (HTuple) 0;
        }
        HTuple htuple14;
        if ((int) new HTuple(htuple11.TupleEqual((HTuple) "true")) != 0)
        {
          htuple14 = (HTuple) 1;
        }
        else
        {
          if ((int) new HTuple(htuple11.TupleEqual((HTuple) "false")) == 0)
            throw new HalconException((HTuple) "Wrong value of control parameter Slant");
          htuple14 = (HTuple) 0;
        }
        try
        {
          HOperatorSet.SetFont(hv_WindowHandle, (HTuple) "-" + t2 + "-" + htuple10 + "-*-" + htuple14 + "-*-*-" + htuple13 + "-");
        }
        catch (HalconException ex)
        {
          ex.ToHTuple(out exception);
        }
      }
      else if ((int) new HTuple(information.TupleSubstr((HTuple) 0, (HTuple) 2).TupleEqual((HTuple) "Dar")) != 0)
      {
        HTuple index1 = (HTuple) 0;
        if ((int) new HTuple(htuple11.TupleEqual((HTuple) "true")) != 0)
          index1 = index1.TupleBor((HTuple) 1);
        else if ((int) new HTuple(htuple11.TupleNotEqual((HTuple) "false")) != 0)
          throw new HalconException((HTuple) "Wrong value of control parameter Slant");
        if ((int) new HTuple(htuple9.TupleEqual((HTuple) "true")) != 0)
          index1 = index1.TupleBor((HTuple) 2);
        else if ((int) new HTuple(htuple9.TupleNotEqual((HTuple) "false")) != 0)
          throw new HalconException((HTuple) "Wrong value of control parameter Bold");
        HTuple htuple12;
        if ((int) new HTuple(t2.TupleEqual((HTuple) "mono")) != 0)
        {
          htuple12 = new HTuple();
          htuple12[0] = (HTupleElements) "Menlo-Regular";
          htuple12[1] = (HTupleElements) "Menlo-Italic";
          htuple12[2] = (HTupleElements) "Menlo-Bold";
          htuple12[3] = (HTupleElements) "Menlo-BoldItalic";
        }
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "Courier")).TupleOr(new HTuple(t2.TupleEqual((HTuple) "courier"))) != 0)
        {
          htuple12 = new HTuple();
          htuple12[0] = (HTupleElements) "CourierNewPSMT";
          htuple12[1] = (HTupleElements) "CourierNewPS-ItalicMT";
          htuple12[2] = (HTupleElements) "CourierNewPS-BoldMT";
          htuple12[3] = (HTupleElements) "CourierNewPS-BoldItalicMT";
        }
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "sans")) != 0)
        {
          htuple12 = new HTuple();
          htuple12[0] = (HTupleElements) "ArialMT";
          htuple12[1] = (HTupleElements) "Arial-ItalicMT";
          htuple12[2] = (HTupleElements) "Arial-BoldMT";
          htuple12[3] = (HTupleElements) "Arial-BoldItalicMT";
        }
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "serif")) != 0)
        {
          htuple12 = new HTuple();
          htuple12[0] = (HTupleElements) "TimesNewRomanPSMT";
          htuple12[1] = (HTupleElements) "TimesNewRomanPS-ItalicMT";
          htuple12[2] = (HTupleElements) "TimesNewRomanPS-BoldMT";
          htuple12[3] = (HTupleElements) "TimesNewRomanPS-BoldItalicMT";
        }
        else
        {
          HOperatorSet.QueryFont(hv_WindowHandle, out font2);
          htuple12 = new HTuple().TupleConcat(t2).TupleConcat(t2).TupleConcat(t2).TupleConcat(t2);
          HTuple htuple13 = new HTuple().TupleConcat(t2).TupleConcat(t2 + "-Regular").TupleConcat(t2 + "MT");
          for (HTuple index2 = (HTuple) 0; (int) index2 <= (int) (new HTuple(htuple13.TupleLength()) - 1); index2 = (HTuple) ((int) index2 + 1))
          {
            HOperatorSet.TupleFind(font2, htuple13.TupleSelect(index2), out indices1);
            if ((int) new HTuple(indices1.TupleNotEqual( -1)) != 0)
            {
              if (htuple12 == null)
                htuple12 = new HTuple();
              htuple12[0] = (HTupleElements) htuple13.TupleSelect(index2);
              break;
            }
          }
          HTuple htuple14 = new HTuple().TupleConcat(t2 + "-Italic").TupleConcat(t2 + "-ItalicMT").TupleConcat(t2 + "-Oblique");
          for (HTuple index2 = (HTuple) 0; (int) index2 <= (int) (new HTuple(htuple14.TupleLength()) - 1); index2 = (HTuple) ((int) index2 + 1))
          {
            HOperatorSet.TupleFind(font2, htuple14.TupleSelect(index2), out indices1);
            if ((int) new HTuple(indices1.TupleNotEqual( -1)) != 0)
            {
              if (htuple12 == null)
                htuple12 = new HTuple();
              htuple12[1] = (HTupleElements) htuple14.TupleSelect(index2);
              break;
            }
          }
          HTuple htuple15 = new HTuple().TupleConcat(t2 + "-Bold").TupleConcat(t2 + "-BoldMT");
          for (HTuple index2 = (HTuple) 0; (int) index2 <= (int) (new HTuple(htuple15.TupleLength()) - 1); index2 = (HTuple) ((int) index2 + 1))
          {
            HOperatorSet.TupleFind(font2, htuple15.TupleSelect(index2), out indices1);
            if ((int) new HTuple(indices1.TupleNotEqual( -1)) != 0)
            {
              if (htuple12 == null)
                htuple12 = new HTuple();
              htuple12[2] = (HTupleElements) htuple15.TupleSelect(index2);
              break;
            }
          }
          HTuple htuple16 = new HTuple().TupleConcat(t2 + "-BoldItalic").TupleConcat(t2 + "-BoldItalicMT").TupleConcat(t2 + "-BoldOblique");
          for (HTuple index2 = (HTuple) 0; (int) index2 <= (int) (new HTuple(htuple16.TupleLength()) - 1); index2 = (HTuple) ((int) index2 + 1))
          {
            HOperatorSet.TupleFind(font2, htuple16.TupleSelect(index2), out indices1);
            if ((int) new HTuple(indices1.TupleNotEqual( -1)) != 0)
            {
              if (htuple12 == null)
                htuple12 = new HTuple();
              htuple12[3] = (HTupleElements) htuple16.TupleSelect(index2);
              break;
            }
          }
        }
        HTuple htuple17 = htuple12.TupleSelect(index1);
        try
        {
          HOperatorSet.SetFont(hv_WindowHandle, htuple17 + "-" + htuple10);
        }
        catch (HalconException ex)
        {
          ex.ToHTuple(out exception);
        }
      }
      else
      {
        HTuple toFind = htuple10 * 1.25;
        HTuple htuple12 = new HTuple();
        htuple12[0] = (HTupleElements) 11;
        htuple12[1] = (HTupleElements) 14;
        htuple12[2] = (HTupleElements) 17;
        htuple12[3] = (HTupleElements) 20;
        htuple12[4] = (HTupleElements) 25;
        htuple12[5] = (HTupleElements) 34;
        if ((int) new HTuple(htuple12.TupleFind(toFind).TupleEqual( -1)) != 0)
        {
          HOperatorSet.TupleSortIndex((htuple12 - toFind).TupleAbs(), out indices2);
          toFind = htuple12.TupleSelect(indices2.TupleSelect((HTuple) 0));
        }
        if ((int) new HTuple(t2.TupleEqual((HTuple) "mono")).TupleOr(new HTuple(t2.TupleEqual((HTuple) "Courier"))) != 0)
          t2 = (HTuple) "courier";
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "sans")) != 0)
          t2 = (HTuple) "helvetica";
        else if ((int) new HTuple(t2.TupleEqual((HTuple) "serif")) != 0)
          t2 = (HTuple) "times";
        HTuple htuple13;
        if ((int) new HTuple(htuple9.TupleEqual((HTuple) "true")) != 0)
        {
          htuple13 = (HTuple) "bold";
        }
        else
        {
          if ((int) new HTuple(htuple9.TupleEqual((HTuple) "false")) == 0)
            throw new HalconException((HTuple) "Wrong value of control parameter Bold");
          htuple13 = (HTuple) "medium";
        }
        HTuple htuple14;
        if ((int) new HTuple(htuple11.TupleEqual((HTuple) "true")) != 0)
        {
          htuple14 = (int) new HTuple(t2.TupleEqual((HTuple) "times")) == 0 ? (HTuple) "o" : (HTuple) "i";
        }
        else
        {
          if ((int) new HTuple(htuple11.TupleEqual((HTuple) "false")) == 0)
            throw new HalconException((HTuple) "Wrong value of control parameter Slant");
          htuple14 = (HTuple) "r";
        }
        try
        {
          HOperatorSet.SetFont(hv_WindowHandle, (HTuple) "-adobe-" + t2 + "-" + htuple13 + "-" + htuple14 + "-normal-*-" + toFind + "-*-*-*-*-*-*-*");
        }
        catch (HalconException ex1)
        {
          ex1.ToHTuple(out exception);
          if ((int) new HTuple(information.TupleSubstr((HTuple) 0, (HTuple) 4).TupleEqual((HTuple) "Linux")).TupleAnd(new HTuple(t2.TupleEqual((HTuple) "courier"))) != 0)
          {
            HOperatorSet.QueryFont(hv_WindowHandle, out font1);
            HTuple expression = (HTuple) "^-[^-]*-[^-]*[Cc]ourier[^-]*-" + htuple13 + "-" + htuple14;
            HTuple htuple15 = font1.TupleRegexpSelect(expression).TupleRegexpMatch(expression);
            if ((int) new HTuple(new HTuple(htuple15.TupleLength()).TupleEqual((HTuple) 0)) != 0)
            {
              HTuple htuple16 = (HTuple) "Wrong font name";
            }
            else
            {
              try
              {
                HOperatorSet.SetFont(hv_WindowHandle, htuple15.TupleSelect((HTuple) 0) + "-normal-*-" + toFind + "-*-*-*-*-*-*-*");
              }
              catch (HalconException ex2)
              {
                ex2.ToHTuple(out exception);
              }
            }
          }
        }
      }
    }
  }
}
