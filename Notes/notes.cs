/*
Todo:

1.
indirect left recursive, action calling

2.
boolean expression,  a is null

3. join, group by, in, with, like

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