using System;
using System.Runtime.InteropServices;

namespace FFmpeg.AutoGen
{
    #pragma warning disable 169
    
    public unsafe struct short_array2
    {
        public static readonly int Size = 2;
        fixed short _[2];
        
        public short this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (short_array2* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (short_array2* p = &this) { p->_[i] = value; } }
        }
        public short[] ToArray()
        {
            fixed (short_array2* p = &this) { var a = new short[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(short[] array)
        {
            fixed (short_array2* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator short[](short_array2 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_ptrArray3
    {
        public static readonly int Size = 3;
        byte* _0; byte* _1; byte* _2;
        
        public byte* this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte** p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte** p0 = &_0) { *(p0 + i) = value;  } }
        }
        public byte*[] ToArray()
        {
            fixed (byte** p0 = &_0) { var a = new byte*[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(byte*[] array)
        {
            fixed (byte** p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator byte*[](byte_ptrArray3 @struct) => @struct.ToArray();
    }
    
    public unsafe struct int_array3
    {
        public static readonly int Size = 3;
        fixed int _[3];
        
        public int this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array3* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array3* p = &this) { p->_[i] = value; } }
        }
        public int[] ToArray()
        {
            fixed (int_array3* p = &this) { var a = new int[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(int[] array)
        {
            fixed (int_array3* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator int[](int_array3 @struct) => @struct.ToArray();
    }
    
    public unsafe struct AVComponentDescriptor_array4
    {
        public static readonly int Size = 4;
        AVComponentDescriptor _0; AVComponentDescriptor _1; AVComponentDescriptor _2; AVComponentDescriptor _3;
        
        public AVComponentDescriptor this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (AVComponentDescriptor* p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (AVComponentDescriptor* p0 = &_0) { *(p0 + i) = value;  } }
        }
        public AVComponentDescriptor[] ToArray()
        {
            fixed (AVComponentDescriptor* p0 = &_0) { var a = new AVComponentDescriptor[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(AVComponentDescriptor[] array)
        {
            fixed (AVComponentDescriptor* p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator AVComponentDescriptor[](AVComponentDescriptor_array4 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_ptrArray4
    {
        public static readonly int Size = 4;
        byte* _0; byte* _1; byte* _2; byte* _3;
        
        public byte* this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte** p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte** p0 = &_0) { *(p0 + i) = value;  } }
        }
        public byte*[] ToArray()
        {
            fixed (byte** p0 = &_0) { var a = new byte*[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(byte*[] array)
        {
            fixed (byte** p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator byte*[](byte_ptrArray4 @struct) => @struct.ToArray();
    }
    
    public unsafe struct int_array4
    {
        public static readonly int Size = 4;
        fixed int _[4];
        
        public int this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array4* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array4* p = &this) { p->_[i] = value; } }
        }
        public int[] ToArray()
        {
            fixed (int_array4* p = &this) { var a = new int[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(int[] array)
        {
            fixed (int_array4* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator int[](int_array4 @struct) => @struct.ToArray();
    }
    
    public unsafe struct long_array4
    {
        public static readonly int Size = 4;
        fixed long _[4];
        
        public long this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (long_array4* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (long_array4* p = &this) { p->_[i] = value; } }
        }
        public long[] ToArray()
        {
            fixed (long_array4* p = &this) { var a = new long[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(long[] array)
        {
            fixed (long_array4* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator long[](long_array4 @struct) => @struct.ToArray();
    }
    
    public unsafe struct int_array5
    {
        public static readonly int Size = 5;
        fixed int _[5];
        
        public int this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array5* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array5* p = &this) { p->_[i] = value; } }
        }
        public int[] ToArray()
        {
            fixed (int_array5* p = &this) { var a = new int[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(int[] array)
        {
            fixed (int_array5* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator int[](int_array5 @struct) => @struct.ToArray();
    }
    
    public unsafe struct short_arrayOfArray6
    {
        public static readonly int Size = 6;
        short_array2 _0; short_array2 _1; short_array2 _2; short_array2 _3; short_array2 _4; short_array2 _5;
        
        public short_array2 this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (short_array2* p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (short_array2* p0 = &_0) { *(p0 + i) = value;  } }
        }
        public short_array2[] ToArray()
        {
            fixed (short_array2* p0 = &_0) { var a = new short_array2[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(short_array2[] array)
        {
            fixed (short_array2* p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator short_array2[](short_arrayOfArray6 @struct) => @struct.ToArray();
    }
    
    public unsafe struct AVBufferRef_ptrArray8
    {
        public static readonly int Size = 8;
        AVBufferRef* _0; AVBufferRef* _1; AVBufferRef* _2; AVBufferRef* _3; AVBufferRef* _4; AVBufferRef* _5; AVBufferRef* _6; AVBufferRef* _7;
        
        public AVBufferRef* this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (AVBufferRef** p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (AVBufferRef** p0 = &_0) { *(p0 + i) = value;  } }
        }
        public AVBufferRef*[] ToArray()
        {
            fixed (AVBufferRef** p0 = &_0) { var a = new AVBufferRef*[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(AVBufferRef*[] array)
        {
            fixed (AVBufferRef** p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator AVBufferRef*[](AVBufferRef_ptrArray8 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_array8
    {
        public static readonly int Size = 8;
        fixed byte _[8];
        
        public byte this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array8* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array8* p = &this) { p->_[i] = value; } }
        }
        public byte[] ToArray()
        {
            fixed (byte_array8* p = &this) { var a = new byte[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(byte[] array)
        {
            fixed (byte_array8* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator byte[](byte_array8 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_ptrArray8
    {
        public static readonly int Size = 8;
        byte* _0; byte* _1; byte* _2; byte* _3; byte* _4; byte* _5; byte* _6; byte* _7;
        
        public byte* this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte** p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte** p0 = &_0) { *(p0 + i) = value;  } }
        }
        public byte*[] ToArray()
        {
            fixed (byte** p0 = &_0) { var a = new byte*[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(byte*[] array)
        {
            fixed (byte** p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator byte*[](byte_ptrArray8 @struct) => @struct.ToArray();
    }
    
    public unsafe struct int_array8
    {
        public static readonly int Size = 8;
        fixed int _[8];
        
        public int this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array8* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (int_array8* p = &this) { p->_[i] = value; } }
        }
        public int[] ToArray()
        {
            fixed (int_array8* p = &this) { var a = new int[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(int[] array)
        {
            fixed (int_array8* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator int[](int_array8 @struct) => @struct.ToArray();
    }
    
    public unsafe struct ulong_array8
    {
        public static readonly int Size = 8;
        fixed ulong _[8];
        
        public ulong this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (ulong_array8* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (ulong_array8* p = &this) { p->_[i] = value; } }
        }
        public ulong[] ToArray()
        {
            fixed (ulong_array8* p = &this) { var a = new ulong[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(ulong[] array)
        {
            fixed (ulong_array8* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator ulong[](ulong_array8 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_array17
    {
        public static readonly int Size = 17;
        fixed byte _[17];
        
        public byte this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array17* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array17* p = &this) { p->_[i] = value; } }
        }
        public byte[] ToArray()
        {
            fixed (byte_array17* p = &this) { var a = new byte[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(byte[] array)
        {
            fixed (byte_array17* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator byte[](byte_array17 @struct) => @struct.ToArray();
    }
    
    public unsafe struct long_array17
    {
        public static readonly int Size = 17;
        fixed long _[17];
        
        public long this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (long_array17* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (long_array17* p = &this) { p->_[i] = value; } }
        }
        public long[] ToArray()
        {
            fixed (long_array17* p = &this) { var a = new long[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(long[] array)
        {
            fixed (long_array17* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator long[](long_array17 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_array32
    {
        public static readonly int Size = 32;
        fixed byte _[32];
        
        public byte this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array32* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array32* p = &this) { p->_[i] = value; } }
        }
        public byte[] ToArray()
        {
            fixed (byte_array32* p = &this) { var a = new byte[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(byte[] array)
        {
            fixed (byte_array32* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator byte[](byte_array32 @struct) => @struct.ToArray();
    }
    
    public unsafe struct double_array399
    {
        public static readonly int Size = 399;
        fixed double _[399];
        
        public double this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (double_array399* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (double_array399* p = &this) { p->_[i] = value; } }
        }
        public double[] ToArray()
        {
            fixed (double_array399* p = &this) { var a = new double[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(double[] array)
        {
            fixed (double_array399* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator double[](double_array399 @struct) => @struct.ToArray();
    }
    
    public unsafe struct double_arrayOfArray798
    {
        public static readonly int Size = 798;
        double_array399 _0; double_array399 _1; double_array399 _2; double_array399 _3; double_array399 _4; double_array399 _5; double_array399 _6; double_array399 _7; double_array399 _8; double_array399 _9; double_array399 _10; double_array399 _11; double_array399 _12; double_array399 _13; double_array399 _14; double_array399 _15; double_array399 _16; double_array399 _17; double_array399 _18; double_array399 _19; double_array399 _20; double_array399 _21; double_array399 _22; double_array399 _23; double_array399 _24; double_array399 _25; double_array399 _26; double_array399 _27; double_array399 _28; double_array399 _29; double_array399 _30; double_array399 _31; double_array399 _32; double_array399 _33; double_array399 _34; double_array399 _35; double_array399 _36; double_array399 _37; double_array399 _38; double_array399 _39; double_array399 _40; double_array399 _41; double_array399 _42; double_array399 _43; double_array399 _44; double_array399 _45; double_array399 _46; double_array399 _47; double_array399 _48; double_array399 _49; double_array399 _50; double_array399 _51; double_array399 _52; double_array399 _53; double_array399 _54; double_array399 _55; double_array399 _56; double_array399 _57; double_array399 _58; double_array399 _59; double_array399 _60; double_array399 _61; double_array399 _62; double_array399 _63; double_array399 _64; double_array399 _65; double_array399 _66; double_array399 _67; double_array399 _68; double_array399 _69; double_array399 _70; double_array399 _71; double_array399 _72; double_array399 _73; double_array399 _74; double_array399 _75; double_array399 _76; double_array399 _77; double_array399 _78; double_array399 _79; double_array399 _80; double_array399 _81; double_array399 _82; double_array399 _83; double_array399 _84; double_array399 _85; double_array399 _86; double_array399 _87; double_array399 _88; double_array399 _89; double_array399 _90; double_array399 _91; double_array399 _92; double_array399 _93; double_array399 _94; double_array399 _95; double_array399 _96; double_array399 _97; double_array399 _98; double_array399 _99; double_array399 _100; double_array399 _101; double_array399 _102; double_array399 _103; double_array399 _104; double_array399 _105; double_array399 _106; double_array399 _107; double_array399 _108; double_array399 _109; double_array399 _110; double_array399 _111; double_array399 _112; double_array399 _113; double_array399 _114; double_array399 _115; double_array399 _116; double_array399 _117; double_array399 _118; double_array399 _119; double_array399 _120; double_array399 _121; double_array399 _122; double_array399 _123; double_array399 _124; double_array399 _125; double_array399 _126; double_array399 _127; double_array399 _128; double_array399 _129; double_array399 _130; double_array399 _131; double_array399 _132; double_array399 _133; double_array399 _134; double_array399 _135; double_array399 _136; double_array399 _137; double_array399 _138; double_array399 _139; double_array399 _140; double_array399 _141; double_array399 _142; double_array399 _143; double_array399 _144; double_array399 _145; double_array399 _146; double_array399 _147; double_array399 _148; double_array399 _149; double_array399 _150; double_array399 _151; double_array399 _152; double_array399 _153; double_array399 _154; double_array399 _155; double_array399 _156; double_array399 _157; double_array399 _158; double_array399 _159; double_array399 _160; double_array399 _161; double_array399 _162; double_array399 _163; double_array399 _164; double_array399 _165; double_array399 _166; double_array399 _167; double_array399 _168; double_array399 _169; double_array399 _170; double_array399 _171; double_array399 _172; double_array399 _173; double_array399 _174; double_array399 _175; double_array399 _176; double_array399 _177; double_array399 _178; double_array399 _179; double_array399 _180; double_array399 _181; double_array399 _182; double_array399 _183; double_array399 _184; double_array399 _185; double_array399 _186; double_array399 _187; double_array399 _188; double_array399 _189; double_array399 _190; double_array399 _191; double_array399 _192; double_array399 _193; double_array399 _194; double_array399 _195; double_array399 _196; double_array399 _197; double_array399 _198; double_array399 _199; double_array399 _200; double_array399 _201; double_array399 _202; double_array399 _203; double_array399 _204; double_array399 _205; double_array399 _206; double_array399 _207; double_array399 _208; double_array399 _209; double_array399 _210; double_array399 _211; double_array399 _212; double_array399 _213; double_array399 _214; double_array399 _215; double_array399 _216; double_array399 _217; double_array399 _218; double_array399 _219; double_array399 _220; double_array399 _221; double_array399 _222; double_array399 _223; double_array399 _224; double_array399 _225; double_array399 _226; double_array399 _227; double_array399 _228; double_array399 _229; double_array399 _230; double_array399 _231; double_array399 _232; double_array399 _233; double_array399 _234; double_array399 _235; double_array399 _236; double_array399 _237; double_array399 _238; double_array399 _239; double_array399 _240; double_array399 _241; double_array399 _242; double_array399 _243; double_array399 _244; double_array399 _245; double_array399 _246; double_array399 _247; double_array399 _248; double_array399 _249; double_array399 _250; double_array399 _251; double_array399 _252; double_array399 _253; double_array399 _254; double_array399 _255; double_array399 _256; double_array399 _257; double_array399 _258; double_array399 _259; double_array399 _260; double_array399 _261; double_array399 _262; double_array399 _263; double_array399 _264; double_array399 _265; double_array399 _266; double_array399 _267; double_array399 _268; double_array399 _269; double_array399 _270; double_array399 _271; double_array399 _272; double_array399 _273; double_array399 _274; double_array399 _275; double_array399 _276; double_array399 _277; double_array399 _278; double_array399 _279; double_array399 _280; double_array399 _281; double_array399 _282; double_array399 _283; double_array399 _284; double_array399 _285; double_array399 _286; double_array399 _287; double_array399 _288; double_array399 _289; double_array399 _290; double_array399 _291; double_array399 _292; double_array399 _293; double_array399 _294; double_array399 _295; double_array399 _296; double_array399 _297; double_array399 _298; double_array399 _299; double_array399 _300; double_array399 _301; double_array399 _302; double_array399 _303; double_array399 _304; double_array399 _305; double_array399 _306; double_array399 _307; double_array399 _308; double_array399 _309; double_array399 _310; double_array399 _311; double_array399 _312; double_array399 _313; double_array399 _314; double_array399 _315; double_array399 _316; double_array399 _317; double_array399 _318; double_array399 _319; double_array399 _320; double_array399 _321; double_array399 _322; double_array399 _323; double_array399 _324; double_array399 _325; double_array399 _326; double_array399 _327; double_array399 _328; double_array399 _329; double_array399 _330; double_array399 _331; double_array399 _332; double_array399 _333; double_array399 _334; double_array399 _335; double_array399 _336; double_array399 _337; double_array399 _338; double_array399 _339; double_array399 _340; double_array399 _341; double_array399 _342; double_array399 _343; double_array399 _344; double_array399 _345; double_array399 _346; double_array399 _347; double_array399 _348; double_array399 _349; double_array399 _350; double_array399 _351; double_array399 _352; double_array399 _353; double_array399 _354; double_array399 _355; double_array399 _356; double_array399 _357; double_array399 _358; double_array399 _359; double_array399 _360; double_array399 _361; double_array399 _362; double_array399 _363; double_array399 _364; double_array399 _365; double_array399 _366; double_array399 _367; double_array399 _368; double_array399 _369; double_array399 _370; double_array399 _371; double_array399 _372; double_array399 _373; double_array399 _374; double_array399 _375; double_array399 _376; double_array399 _377; double_array399 _378; double_array399 _379; double_array399 _380; double_array399 _381; double_array399 _382; double_array399 _383; double_array399 _384; double_array399 _385; double_array399 _386; double_array399 _387; double_array399 _388; double_array399 _389; double_array399 _390; double_array399 _391; double_array399 _392; double_array399 _393; double_array399 _394; double_array399 _395; double_array399 _396; double_array399 _397; double_array399 _398; double_array399 _399; double_array399 _400; double_array399 _401; double_array399 _402; double_array399 _403; double_array399 _404; double_array399 _405; double_array399 _406; double_array399 _407; double_array399 _408; double_array399 _409; double_array399 _410; double_array399 _411; double_array399 _412; double_array399 _413; double_array399 _414; double_array399 _415; double_array399 _416; double_array399 _417; double_array399 _418; double_array399 _419; double_array399 _420; double_array399 _421; double_array399 _422; double_array399 _423; double_array399 _424; double_array399 _425; double_array399 _426; double_array399 _427; double_array399 _428; double_array399 _429; double_array399 _430; double_array399 _431; double_array399 _432; double_array399 _433; double_array399 _434; double_array399 _435; double_array399 _436; double_array399 _437; double_array399 _438; double_array399 _439; double_array399 _440; double_array399 _441; double_array399 _442; double_array399 _443; double_array399 _444; double_array399 _445; double_array399 _446; double_array399 _447; double_array399 _448; double_array399 _449; double_array399 _450; double_array399 _451; double_array399 _452; double_array399 _453; double_array399 _454; double_array399 _455; double_array399 _456; double_array399 _457; double_array399 _458; double_array399 _459; double_array399 _460; double_array399 _461; double_array399 _462; double_array399 _463; double_array399 _464; double_array399 _465; double_array399 _466; double_array399 _467; double_array399 _468; double_array399 _469; double_array399 _470; double_array399 _471; double_array399 _472; double_array399 _473; double_array399 _474; double_array399 _475; double_array399 _476; double_array399 _477; double_array399 _478; double_array399 _479; double_array399 _480; double_array399 _481; double_array399 _482; double_array399 _483; double_array399 _484; double_array399 _485; double_array399 _486; double_array399 _487; double_array399 _488; double_array399 _489; double_array399 _490; double_array399 _491; double_array399 _492; double_array399 _493; double_array399 _494; double_array399 _495; double_array399 _496; double_array399 _497; double_array399 _498; double_array399 _499; double_array399 _500; double_array399 _501; double_array399 _502; double_array399 _503; double_array399 _504; double_array399 _505; double_array399 _506; double_array399 _507; double_array399 _508; double_array399 _509; double_array399 _510; double_array399 _511; double_array399 _512; double_array399 _513; double_array399 _514; double_array399 _515; double_array399 _516; double_array399 _517; double_array399 _518; double_array399 _519; double_array399 _520; double_array399 _521; double_array399 _522; double_array399 _523; double_array399 _524; double_array399 _525; double_array399 _526; double_array399 _527; double_array399 _528; double_array399 _529; double_array399 _530; double_array399 _531; double_array399 _532; double_array399 _533; double_array399 _534; double_array399 _535; double_array399 _536; double_array399 _537; double_array399 _538; double_array399 _539; double_array399 _540; double_array399 _541; double_array399 _542; double_array399 _543; double_array399 _544; double_array399 _545; double_array399 _546; double_array399 _547; double_array399 _548; double_array399 _549; double_array399 _550; double_array399 _551; double_array399 _552; double_array399 _553; double_array399 _554; double_array399 _555; double_array399 _556; double_array399 _557; double_array399 _558; double_array399 _559; double_array399 _560; double_array399 _561; double_array399 _562; double_array399 _563; double_array399 _564; double_array399 _565; double_array399 _566; double_array399 _567; double_array399 _568; double_array399 _569; double_array399 _570; double_array399 _571; double_array399 _572; double_array399 _573; double_array399 _574; double_array399 _575; double_array399 _576; double_array399 _577; double_array399 _578; double_array399 _579; double_array399 _580; double_array399 _581; double_array399 _582; double_array399 _583; double_array399 _584; double_array399 _585; double_array399 _586; double_array399 _587; double_array399 _588; double_array399 _589; double_array399 _590; double_array399 _591; double_array399 _592; double_array399 _593; double_array399 _594; double_array399 _595; double_array399 _596; double_array399 _597; double_array399 _598; double_array399 _599; double_array399 _600; double_array399 _601; double_array399 _602; double_array399 _603; double_array399 _604; double_array399 _605; double_array399 _606; double_array399 _607; double_array399 _608; double_array399 _609; double_array399 _610; double_array399 _611; double_array399 _612; double_array399 _613; double_array399 _614; double_array399 _615; double_array399 _616; double_array399 _617; double_array399 _618; double_array399 _619; double_array399 _620; double_array399 _621; double_array399 _622; double_array399 _623; double_array399 _624; double_array399 _625; double_array399 _626; double_array399 _627; double_array399 _628; double_array399 _629; double_array399 _630; double_array399 _631; double_array399 _632; double_array399 _633; double_array399 _634; double_array399 _635; double_array399 _636; double_array399 _637; double_array399 _638; double_array399 _639; double_array399 _640; double_array399 _641; double_array399 _642; double_array399 _643; double_array399 _644; double_array399 _645; double_array399 _646; double_array399 _647; double_array399 _648; double_array399 _649; double_array399 _650; double_array399 _651; double_array399 _652; double_array399 _653; double_array399 _654; double_array399 _655; double_array399 _656; double_array399 _657; double_array399 _658; double_array399 _659; double_array399 _660; double_array399 _661; double_array399 _662; double_array399 _663; double_array399 _664; double_array399 _665; double_array399 _666; double_array399 _667; double_array399 _668; double_array399 _669; double_array399 _670; double_array399 _671; double_array399 _672; double_array399 _673; double_array399 _674; double_array399 _675; double_array399 _676; double_array399 _677; double_array399 _678; double_array399 _679; double_array399 _680; double_array399 _681; double_array399 _682; double_array399 _683; double_array399 _684; double_array399 _685; double_array399 _686; double_array399 _687; double_array399 _688; double_array399 _689; double_array399 _690; double_array399 _691; double_array399 _692; double_array399 _693; double_array399 _694; double_array399 _695; double_array399 _696; double_array399 _697; double_array399 _698; double_array399 _699; double_array399 _700; double_array399 _701; double_array399 _702; double_array399 _703; double_array399 _704; double_array399 _705; double_array399 _706; double_array399 _707; double_array399 _708; double_array399 _709; double_array399 _710; double_array399 _711; double_array399 _712; double_array399 _713; double_array399 _714; double_array399 _715; double_array399 _716; double_array399 _717; double_array399 _718; double_array399 _719; double_array399 _720; double_array399 _721; double_array399 _722; double_array399 _723; double_array399 _724; double_array399 _725; double_array399 _726; double_array399 _727; double_array399 _728; double_array399 _729; double_array399 _730; double_array399 _731; double_array399 _732; double_array399 _733; double_array399 _734; double_array399 _735; double_array399 _736; double_array399 _737; double_array399 _738; double_array399 _739; double_array399 _740; double_array399 _741; double_array399 _742; double_array399 _743; double_array399 _744; double_array399 _745; double_array399 _746; double_array399 _747; double_array399 _748; double_array399 _749; double_array399 _750; double_array399 _751; double_array399 _752; double_array399 _753; double_array399 _754; double_array399 _755; double_array399 _756; double_array399 _757; double_array399 _758; double_array399 _759; double_array399 _760; double_array399 _761; double_array399 _762; double_array399 _763; double_array399 _764; double_array399 _765; double_array399 _766; double_array399 _767; double_array399 _768; double_array399 _769; double_array399 _770; double_array399 _771; double_array399 _772; double_array399 _773; double_array399 _774; double_array399 _775; double_array399 _776; double_array399 _777; double_array399 _778; double_array399 _779; double_array399 _780; double_array399 _781; double_array399 _782; double_array399 _783; double_array399 _784; double_array399 _785; double_array399 _786; double_array399 _787; double_array399 _788; double_array399 _789; double_array399 _790; double_array399 _791; double_array399 _792; double_array399 _793; double_array399 _794; double_array399 _795; double_array399 _796; double_array399 _797;
        
        public double_array399 this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (double_array399* p0 = &_0) { return *(p0 + i); } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (double_array399* p0 = &_0) { *(p0 + i) = value;  } }
        }
        public double_array399[] ToArray()
        {
            fixed (double_array399* p0 = &_0) { var a = new double_array399[Size]; for (uint i = 0; i < Size; i++) a[i] = *(p0 + i); return a; }
        }
        public void UpdateFrom(double_array399[] array)
        {
            fixed (double_array399* p0 = &_0) { uint i = 0; foreach(var value in array) { *(p0 + i++) = value; if (i >= Size) return; } }
        }
        public static implicit operator double_array399[](double_arrayOfArray798 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_array1024
    {
        public static readonly int Size = 1024;
        fixed byte _[1024];
        
        public byte this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array1024* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array1024* p = &this) { p->_[i] = value; } }
        }
        public byte[] ToArray()
        {
            fixed (byte_array1024* p = &this) { var a = new byte[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(byte[] array)
        {
            fixed (byte_array1024* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator byte[](byte_array1024 @struct) => @struct.ToArray();
    }
    
    public unsafe struct byte_array61440
    {
        public static readonly int Size = 61440;
        fixed byte _[61440];
        
        public byte this[uint i]
        {
            get { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array61440* p = &this) { return p->_[i]; } }
            set { if (i > Size) throw new ArgumentOutOfRangeException(); fixed (byte_array61440* p = &this) { p->_[i] = value; } }
        }
        public byte[] ToArray()
        {
            fixed (byte_array61440* p = &this) { var a = new byte[Size]; for (uint i = 0; i < Size; i++) a[i] = p->_[i]; return a; }
        }
        public void UpdateFrom(byte[] array)
        {
            fixed (byte_array61440* p = &this) { uint i = 0; foreach(var value in array) { p->_[i++] = value; if (i >= Size) return; } }
        }
        public static implicit operator byte[](byte_array61440 @struct) => @struct.ToArray();
    }
    
}
