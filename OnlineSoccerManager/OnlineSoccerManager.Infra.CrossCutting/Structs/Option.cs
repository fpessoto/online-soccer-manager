﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSoccerManager.Infra.CrossCutting.Structs
{
    using static Helpers;
    public partial struct Option<T>
    {
        public T Value { get; internal set; }
        public bool IsSome { get; }
        public bool IsNone => !IsSome;

        public Option(T value, bool isSome)
        {
            Value = value;
            IsSome = isSome;
        }

        public TR Match<TR>(Func<T, TR> some, Func<TR> none) => IsSome ? some(Value) : none();

        public static readonly Option<T> None = new Option<T>();
        public static implicit operator Option<T>(T value) => Some(value);
        public static implicit operator Option<T>(NoneType _) => None;
    }
    public struct NoneType { }
    public static partial class Helpers
    {
        public static Option<T> Some<T>(T value) => Option.Of(value);

        public static readonly NoneType None = new NoneType();
    }
}
