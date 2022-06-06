using System;
using System.Collections.Generic;
using System.Collections;

namespace NoshScript
{		
	public class Pattern : IEnumerable<PatternInput>
	{
		private List<PatternInput> inputs = new List<PatternInput> ();

		public IEnumerator<PatternInput> GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		public Pattern ()
		{
			
		}

		public void Add (char start, char end)
		{
			inputs.Add (new PatternInput (start, end));
		}

		private PatternInput Get (char key)
		{
			foreach (PatternInput input in inputs) {
				if (input.getStart () == key)
					return input;
			}
			return null;
		}

		public void Compile (string data)
		{
			List<string> currentData = new List<string> ();
			List<PatternInput> inps = new List<PatternInput> ();
			int pointer = 0;

			for (int i = 0; i < data.Length; i++) {
				PatternInput input = Get (data [i]);
				if (input != null) {
					Console.WriteLine ("input : {0}", data [i]);

					inps.Add (input);
					pointer++;
					Console.WriteLine ("count : {0}", pointer);
				}

				if (inps.Count > 0) {

					Console.Write (data [i]);

				}
			}
			Console.WriteLine ();
		}
	}

	public class PatternInput
	{
		private char start, end;

		public PatternInput (char start, char end)
		{
			this.start = start;
			this.end = end;
		}

		public string compile(string data)
		{
			int count = 0;
			string result = "";

			for (int i = 0; i < data.Length; i++) {
				if(count != 0)
					result += data[i];

				if (data [i] == start) {
					count++;
					continue;
				}

				if (data [i] == end) {
					count--;

					if (count == 0) {
						result = result.Remove (result.Length - 1,1);
						break;
					}
				}
			}
		
			return result;
		}

		public char getStart ()
		{
			return start;
		}

		public char getEnd ()
		{
			return end;
		}
	}

	public class Machine
	{
		public static string[] split(string input)
		{
			List<string> datos = new List<string> ();

			string[] inputArray = input.Split (' ');

			string value = "";
			bool end = false;

			for (int i = 0; i < inputArray.Length; i++) 
			{
				if (inputArray [i].Trim ().StartsWith ("\""))
				{
					end = !end;

					if (!end) {
						value += inputArray [i];
						datos.Add (value);
						value = "";
						continue;
					}
				}

				if (end)
				{
					value += inputArray [i];
				}
				else {
					datos.Add (inputArray[i]);
				}
			}

			return datos.ToArray();
		}
	}
}

