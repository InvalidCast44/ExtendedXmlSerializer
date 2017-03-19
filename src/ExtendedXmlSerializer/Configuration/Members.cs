// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExtendedXmlSerialization.Core.Sources;

namespace ExtendedXmlSerialization.Configuration
{
	public class Members<T, TMember> : ReferenceCacheBase<MemberInfo, MemberConfiguration<T, TMember>>,
	                                   IEnumerable<IMemberConfiguration>
	{
		public static IParameterizedSource<IConfiguration, Members<T, TMember>> Defaults { get; } =
			new ReferenceCache<IConfiguration, Members<T, TMember>>(x => new Members<T, TMember>(x));

		Members(IConfiguration configuration) : this(TypeConfigurations<T>.Default.Get(configuration)) {}

		readonly TypeConfiguration<T> _type;
		readonly ICollection<IMemberConfiguration> _members = new HashSet<IMemberConfiguration>();

		public Members(TypeConfiguration<T> type)
		{
			_type = type;
		}

		protected override MemberConfiguration<T, TMember> Create(MemberInfo parameter)
		{
			var result = new MemberConfiguration<T, TMember>(_type.Member(parameter), _type);
			_members.Add(result);
			return result;
		}

		public IEnumerator<IMemberConfiguration> GetEnumerator() => _members.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}