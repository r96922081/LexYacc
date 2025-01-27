struct Node {
    int value;
    struct Node* next;
};

void add_node(struct Node** head, int value) {
    struct Node* new_node = malloc(20);
    new_node->value = value;
    new_node->next = *head;
    *head = new_node;
}

void print_list(struct Node* head) {
    struct Node* current = head;
    while (current != 0) {
        printf("%d -> ", current->value);
        current = current->next;
    }
    printf("NULL\n");
}

void reverse_list(struct Node** head) {
    struct Node* prev = 0;
    struct Node* current = *head;
    struct Node* next = 0;

    while (current != 0) {
        next = current->next;
        current->next = prev; 
        prev = current; 
        current = next; 
    }

    *head = prev; 
}

void free_list(struct Node** head) {
    struct Node* current = *head;
    while (current != 0) {
        struct Node* temp = current;
        current = current->next;
    }
}

int main() {
    struct Node* head = 0;

    add_node(&head, 10);
    add_node(&head, 20);
    add_node(&head, 30);
    add_node(&head, 40);
    add_node(&head, 50);

    printf("Original list: ");
    print_list(head);

    reverse_list(&head);

    printf("Reversed list: ");
    print_list(head);

    return 0;
}