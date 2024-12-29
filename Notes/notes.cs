/*
Todo:

1.
indirect left recursive, action calling

- join, group by, in, with

- group by min, max, count, sum

- case when

- UPPER, LOWER, LENGTH

- select literal like 1 + 2 + 3, 'BAC' || 'def'

 */

/*
Term:

A: 'B' c | 'D' e

Production rule = A: 'B' c | 'D' e
Production body = 'B' c | 'D' e
Production = 'B' c
Production = 'D' e

*/


/*
not support:

1. 
Do not support empty rule like a: | 'A'
empty rule is used only in left recursion

 */