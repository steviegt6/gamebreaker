/* MIT License
 * 
 * Copyright (c) 2023 Tomat & GameBreaker Contributors
 * Copyright (c) 2020 colinator27
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace GameBreaker.Models
{
    /// <summary>
    /// Contains a GameMaker script.
    /// </summary>
    public class GMScript : IGMSerializable
    {
        public GMString Name;
        public int CodeID;
        public bool Constructor;

        public void Serialize(GMDataWriter writer)
        {
            writer.WritePointerString(Name);
            if (Constructor)
                writer.Write((uint)CodeID | 2147483648u);
            else
                writer.Write(CodeID);
        }

        public void Deserialize(GMDataReader reader)
        {
            Name = reader.ReadStringPointerObject();
            CodeID = reader.ReadInt32();
            if (CodeID < -1)
            {
                // New GMS 2.3 constructor scripts
                Constructor = true;
                CodeID = (int)((uint)CodeID & 2147483647u);
            }
        }

        public override string ToString()
        {
            return $"Script: \"{Name.Content}\"";
        }
    }
}
