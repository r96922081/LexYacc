/*

#int main()
#{
#  int a = 10;
#  int b = 2;
#  int c = a - 3 + b - 5; // 4
#  return c - 1; // 3
#}


.data

.text

.global main

main:
push %rbp
mov %rsp, %rbp
mov $-24, %rsp # reserve 3 variable
mov $10, %rax  # a = 10
mov %rax, -8(%rbp)

mov $2, %rax  # b = 2
mov %rax, -16(%rbp)

# c= a - 3 + b - 5
mov -8(%rbp), %rax

mov $3, %rbx
sub %rbx, %rax

mov -16(%rbp), %rbx
add %rbx, %rax

mov $5, %rbx
sub %rbx, %rax

mov %rax, -24(%rbp)


# return c - 1

mov -24(%rbp), %rax
mov $1, %rbx
sub %rbx, %rax


leave
ret


 */ 