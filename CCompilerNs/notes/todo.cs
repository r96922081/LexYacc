/* 
==
not supported:
- pointer
- macro
- %
- variable in {} scope
- union
- typedef
- double
- Parenthsis in boolean expression, ex: "if ((a>b) || (b < c && d > e))".  but "if (a > b || b > c && d <e)" is ok
- declare variable in for, ex: for (int i = 0; i < 10; i++)
 */ 