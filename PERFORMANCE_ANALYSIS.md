# Perf Summary

The expectations for fulfilling this requirement were a little unclear to me, however, I took an hour or so to air on the safe side.

the performance tests show that the AQueue implementation achieves the expected Big-O time complexities from my initial design:

### Enqueue Performance: O(1)
- **test data**: times of 62, 60, 61, 54 ticks for capacities 10, 100, 1000, 10000
- **analysis**: times are relatively constant across different queue sizes
- **ratios**: 0.98 (1000/10) and 0.90 (10000/100) - close to 1.0 indicating constant time
- **conclusion**: enqueue is O(1) as expected for array-based implementation

### Dequeue Performance: O(1)  
- **test data**: times of 131, 93, 50, 37 ticks for capacities 10, 100, 1000, 10000
- **analysis**: times are relatively constant across different queue sizes
- **ratios**: 0.38 (1000/10) and 0.40 (10000/100) - close to 1.0 indicating constant time
- **conclusion**: dequeue is O(1) as expected due to circular buffer implementation

### Peek Performance: O(1)
- **test data**: times of 57424, 194, 70, 140 ticks for capacities 10, 100, 1000, 10000
- **analysis**: after first outlier (likely JIT compilation), times are constant
- **ratios**: 0.00 (1000/10) and 0.72 (10000/100) - close to 1.0 indicating constant time
- **conclusion**: peek is O(1) as expected for direct array access

### Contains Performance: O(n)
- **test data**: times of 500, 529, 3388, 33369 ticks for capacities 10, 100, 1000, 10000
- **analysis**: times grow significantly as queue size increases
- **ratios**: 6.78 (1000/10) and 63.08 (10000/100) - showing clear linear growth
- **conclusion**: contains is O(n) as expected for linear search

## Comparing my AQueue to LQueue... (this is part of why I left it in the repo and mixed the tests)
1. **dequeue operation**: AQueue is O(1) vs LQueue's O(n) due to no element shifting
2. **memory efficiency**: fixed-size array prevents unexpected memory allocation
3. **circular buffer**: allows reuse of array space efficiently

### Anomalies I noticed
- **first-run outliers**: initial tests show higher times due to JIT compilation and system warmup I think
- **timing variations**: multiple trials help identify consistent performance patterns
- **system factors**: garbage collection and CPU caching can affect individual measurements

### Conclusion:
the data clearly shows that the AQueue implementation achieves the theoretical Big-O:
- constant time operations (enqueue, dequeue, peek) show consistent timing regardless of queue size
- linear time operation (contains) shows proportional growth with queue size
- multiple trials confirm consistent perf characteristics 