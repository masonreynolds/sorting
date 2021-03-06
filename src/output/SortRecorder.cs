using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using assignment1.sorting;
using CsvHelper;

namespace assignment1.output
{
	public static class SortRecorder
	{
		public static void Run()
		{
			// Create a string builder to hold the log messages
			var sb = new StringBuilder();

			_writeMessage("ITCS 6114 Algorithms & Data Structures Assignment 1", sb);
			_writeMessage("This will take a while! When this program finishes execution, a CSV will be produced in this directory displaying the outputs.", sb);

			// Hold a list of CSV records for the output
			var csvRecords = new List<CSVRecord>();
			// Store all the input sizes which will be recorded
			var inputSizes = new int[] { 10, 100, 1000, 1500, 2000, 2500, 4000, 4500, 5000, 7500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 100000 };
			// Create a stopwatch to record sorting times
			var stopWatch = new Stopwatch();
			// Create a rng
			var rand = new Random();
			// Run 5 iterations
			for (int i = 1; i <= 5; i++)
			{
				_writeMessage($"Starting Test Iteration {i}", sb);
				// Run for each input size
				foreach (var n in inputSizes)
				{
					var record = new CSVRecord { Iteration = i, NumElements = n };
					_writeMessage($"\tInput Size n = {n}", sb);
					_writeMessage($"\tGenerating {n} random numbers", sb);
					var randomNumbers = Enumerable.Range(0, n).Select(r => rand.Next(-1000, 1001)).ToList();

					// Test Insertion Sort
					_writeMessage($"\t\tStarting Insertion Sort with n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var insertionResults = InsertionSort.Sort(randomNumbers);

					stopWatch.Stop();
					record.InsertionSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Insertion Sort with n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool insertionSuccess = _isSortCorrect(insertionResults);
					if (insertionSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Merge Sort
					_writeMessage($"\t\tStarting Merge Sort with n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var MergeResults = MergeSort<int>.Sort(randomNumbers);

					stopWatch.Stop();
					record.MergeSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Merge Sort with n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool mergeSuccess = _isSortCorrect(MergeResults);
					if (mergeSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Heap Sort
					_writeMessage($"\t\tStarting Heap Sort with n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var heapResults = HeapSort.Sort(randomNumbers);

					stopWatch.Stop();
					record.HeapSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Heap Sort with n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					bool heapSuccess = _isSortCorrect(heapResults);
					if (heapSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Quick Sort (In-Place)
					_writeMessage($"\t\tStarting Quick Sort (In-Place) with n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var IPQuickSortResults = IPQuickSort<int>.Sort(randomNumbers);

					stopWatch.Stop();
					record.QuickSortInPlaceTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Quick Sort (In-Place) with n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool ipquickSuccess = _isSortCorrect(IPQuickSortResults);
					if (ipquickSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Quick Sort (Modified)
					_writeMessage($"\t\tStarting Quick Sort (Modified) with n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var ModQuickSortResults = ModQuickSort<int>.Sort(randomNumbers);

					stopWatch.Stop();
					record.QuickSortModifiedTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Quick Sort (Modified) with n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool modquickSuccess = _isSortCorrect(ModQuickSortResults);
					if (modquickSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					csvRecords.Add(record);
				}
				_writeMessage($"Concluded Test Iteration {i}", sb);
			}

			_writeMessage("All sorting tests concluded! Please check the project directory for the sorting_results.csv and output.txt files.", sb);

			Console.WriteLine("Writing to sorting_results.csv ...");

			// Write results to CSV
			using (var writer = new StreamWriter("sorting_results.csv"))
			using (var csv = new CsvWriter(writer))
			{
				csv.WriteRecords<CSVRecord>(csvRecords);
			}

			Console.WriteLine("Writing to output.txt ...");

			using (var writer = new StreamWriter("output.txt"))
			{
				writer.Write(sb.ToString());
			}

			Console.WriteLine("All writing successful!");
		}

		public static void RunEdgeCases()
		{
			// Create a string builder to hold the log messages
			var sb = new StringBuilder();

			_writeMessage("ITCS 6114 Algorithms & Data Structures Assignment 1: Edge Cases", sb);
			_writeMessage("This will take a while! When this program finishes execution, a CSV will be produced in this directory displaying the outputs.", sb);

			// Create two lists of CSV records, one for each edge case
			var csvSortedRecords = new List<CSVRecord>();
			var csvReversedRecords = new List<CSVRecord>();
			// Create list of input sizes
			var inputSizes = new int[] { 10, 100, 1000, 1500, 2000, 2500, 4000, 4500, 5000, 7500, 10000, 15000, 20000, 25000, 30000, 35000, 40000, 45000, 50000, 100000 };
			// Create stopwatch
			var stopWatch = new Stopwatch();
			// Create rng
			var rand = new Random();
			// Run 5 iterations
			for (int i = 1; i <= 5; i++)
			{
				_writeMessage($"Starting Test Iteration {i}", sb);
				// Run for each input size
				foreach (var n in inputSizes)
				{
					var sortedRecord = new CSVRecord { Iteration = i, NumElements = n };
					var reversedRecord = new CSVRecord { Iteration = i, NumElements = n };
					_writeMessage($"\tInput Size n = {n}", sb);
					_writeMessage($"\tGenerating {n} random numbers", sb);
					var lowestToHighest = Enumerable.Range(0, n).ToList();
					var highestToLowest = Enumerable.Range(0, n).ToList();
					highestToLowest.Reverse();

					// Test Insertion Sort with sorted List
					_writeMessage($"\t\tStarting Insertion Sort with sorted List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var insertionResults = InsertionSort.Sort(lowestToHighest);

					stopWatch.Stop();
					sortedRecord.InsertionSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Insertion Sort with sorted List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool insertionSuccess = _isSortCorrect(insertionResults);
					if (insertionSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					_writeMessage($"\t\tStarting Insertion Sort with reversed List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					insertionResults = InsertionSort.Sort(highestToLowest);

					stopWatch.Stop();
					reversedRecord.InsertionSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Insertion Sort with reversed List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					insertionSuccess = _isSortCorrect(insertionResults);
					if (insertionSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Merge Sort
					_writeMessage($"\t\tStarting Merge Sort with sorted List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var MergeResults = MergeSort<int>.Sort(lowestToHighest);

					stopWatch.Stop();
					sortedRecord.MergeSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Merge Sort with sorted List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool mergeSuccess = _isSortCorrect(MergeResults);
					if (mergeSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					_writeMessage($"\t\tStarting Merge Sort with reversed List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					MergeResults = MergeSort<int>.Sort(highestToLowest);

					stopWatch.Stop();
					reversedRecord.MergeSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Merge Sort with reversed List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					mergeSuccess = _isSortCorrect(MergeResults);
					if (mergeSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Heap Sort
					_writeMessage($"\t\tStarting Heap Sort with worted List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var heapResults = HeapSort.Sort(lowestToHighest);

					stopWatch.Stop();
					sortedRecord.HeapSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Heap Sort with sorted List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					bool heapSuccess = _isSortCorrect(heapResults);
					if (heapSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					_writeMessage($"\t\tStarting Heap Sort with reversed List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					heapResults = HeapSort.Sort(highestToLowest);

					stopWatch.Stop();
					reversedRecord.HeapSortTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Heap Sort with n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					heapSuccess = _isSortCorrect(heapResults);
					if (heapSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Quick Sort (In-Place)
					_writeMessage($"\t\tStarting Quick Sort (In-Place) with sorted List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var IPQuickSortResults = IPQuickSort<int>.Sort(lowestToHighest);

					stopWatch.Stop();
					sortedRecord.QuickSortInPlaceTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Quick Sort (In-Place) with sorted List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool ipquickSuccess = _isSortCorrect(IPQuickSortResults);
					if (ipquickSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					_writeMessage($"\t\tStarting Quick Sort (In-Place) with reversed List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					IPQuickSortResults = IPQuickSort<int>.Sort(highestToLowest);

					stopWatch.Stop();
					reversedRecord.QuickSortInPlaceTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Quick Sort (In-Place) with reversed List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					ipquickSuccess = _isSortCorrect(IPQuickSortResults);
					if (ipquickSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					// Test Quick Sort (Modified)
					_writeMessage($"\t\tStarting Quick Sort (Modified) with sorted List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					var ModQuickSortResults = ModQuickSort<int>.Sort(lowestToHighest);

					stopWatch.Stop();
					sortedRecord.QuickSortModifiedTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Quick Sort (Modified) with sorted List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					bool modquickSuccess = _isSortCorrect(ModQuickSortResults);
					if (modquickSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					_writeMessage($"\t\tStarting Quick Sort (Modified) with reversed List of n = {n}", sb);
					_writeMessage("\t\t\tRunning...", sb);
					stopWatch.Restart();

					ModQuickSortResults = ModQuickSort<int>.Sort(lowestToHighest);

					stopWatch.Stop();
					reversedRecord.QuickSortModifiedTime = stopWatch.ElapsedMilliseconds;
					_writeMessage($"\t\t\tCompleted Quick Sort (Modified) with reversed List of n = {n} with {stopWatch.ElapsedMilliseconds}ms", sb);
					_writeMessage("\t\t\tVerifying sort results...", sb);
					modquickSuccess = _isSortCorrect(ModQuickSortResults);
					if (modquickSuccess)
					{
						_writeMessage("\t\t\tSort results verified!", sb);
					}
					else
					{
						_writeMessage("\t\t\tSort Failed!", sb);
						return;
					}

					csvSortedRecords.Add(sortedRecord);
					csvReversedRecords.Add(reversedRecord);
				}
				_writeMessage($"Concluded Test Iteration {i}", sb);
			}

			_writeMessage("All sorting tests concluded! Please check the project directory for the sorting_results.csv and output.txt files.", sb);

			Console.WriteLine("Writing to sorting_results.csv ...");

			// Write results to CSV
			using (var writer = new StreamWriter("sorting_sorted_results.csv"))
			using (var csv = new CsvWriter(writer))
			{
				csv.WriteRecords<CSVRecord>(csvSortedRecords);
			}

			using (var writer = new StreamWriter("sorting_reversed_results.csv"))
			using (var csv = new CsvWriter(writer))
			{
				csv.WriteRecords<CSVRecord>(csvReversedRecords);
			}

			Console.WriteLine("Writing to output-edgecases.txt ...");

			using (var writer = new StreamWriter("output-edgecases.txt"))
			{
				writer.Write(sb.ToString());
			}

			Console.WriteLine("All writing successful!");
		}

		// Ensure that the sort actually worked
		private static bool _isSortCorrect(List<int> results)
		{
			for (int i = 1; i < results.Count; i++)
			{
				if (results[i] < results[i - 1])
				{
					return false;
				}
			}

			return true;
		}

		// Write a message to the console as well as append it to a string builder
		private static void _writeMessage(string message, StringBuilder stringBuilder)
		{
			Console.WriteLine(message);
			stringBuilder.AppendLine(message);
		}
	}
}
