/*
Todo:

indirect left recursive, action calling

===

Term:

A: 'B' c | 'D' e

Production rule = A: 'B' c | 'D' e
Production body = 'B' c | 'D' e
Production = 'B' c
Production = 'D' e

===
empty rule is not supported correctly, used only in left recursive internal translation

 */