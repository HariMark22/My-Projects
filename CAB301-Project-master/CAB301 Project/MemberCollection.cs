//CAB301 assessment 1 - 2022
//The implementation of MemberCollection ADT
using System;
using System.Linq;


class MemberCollection : IMemberCollection
{
    // Fields
    private int capacity;
    private int count;
    private Member[] members; //make sure members are sorted in dictionary order

    // Properties

    // get the capacity of this member colllection 
    // pre-condition: nil
    // post-condition: return the capacity of this member collection and this member collection remains unchanged
    public int Capacity { get { return capacity; } }

    // get the number of members in this member colllection 
    // pre-condition: nil
    // post-condition: return the number of members in this member collection and this member collection remains unchanged
    public int Number { get { return count; } }




    // Constructor - to create an object of member collection 
    // Pre-condition: capacity > 0
    // Post-condition: an object of this member collection class is created

    public MemberCollection(int capacity)
    {
        if (capacity > 0)
        {
            this.capacity = capacity;
            members = new Member[capacity];
            count = 0;
        }
    }

    // check if this member collection is full
    // Pre-condition: nil
    // Post-condition: return ture if this member collection is full; otherwise return false.
    public bool IsFull()
    {
        return count == capacity;
    }

    // check if this member collection is empty
    // Pre-condition: nil
    // Post-condition: return ture if this member collection is empty; otherwise return false.
    public bool IsEmpty()
    {
        return count == 0;
    }

    // Add a new member to this member collection
    // Pre-condition: this member collection is not full
    // Post-condition: a new member is added to the member collection and the members are sorted in ascending order by their full names;
    // No duplicate will be added into this the member collection
    public void Add(IMember member)
    {
        // To be implemented by students in Phase 1
        Member another = (Member)member;

        //
        if (!IsFull() && !Search(another))
        {
            int i = count - 1;
            //used to find specific location
            int temp = 0;

            if (count > 0)
            {
                temp = members[i].CompareTo(another);
            }

            while (i >= 0 && temp > 0)
            {
                members[i + 1] = members[i];
                i--;
                if (i >= 0) temp = members[i].CompareTo(another);

            }
            members[i + 1] = another;
            count++;

        }

        else {

            Console.WriteLine("Member already in the collection. Hence, will not be registered");
        
        }

    }

    // Remove a given member out of this member collection
    // Pre-condition: nil
    // Post-condition: the given member has been removed from this member collection, if the given meber was in the member collection
    public void Delete(IMember aMember)
    {
        // To be implemented by students in Phase 1
        Member member = (Member)aMember;
        int i = 0;
        for (; i < count; i++)
        {
            if (member.CompareTo(members[i]) == 0)
            {
                //once a match is found, break. Then the next if statement will trigger
                break;
            }

        }
        if (i < count)
        {
            count--;
            for (int j = i; j < count; j++)
            {
                members[j] = members[j + 1];
            }
        }
    }




    // Search a given member in this member collection 
    // Pre-condition: nil
    // Post-condition: return true if this memeber is in the member collection; return false otherwise; member collection remains unchanged
    public bool Search(IMember member)
    {
        // To be implemented by students in Phase 1
        int leftPoint = 0;
        int rightPoint = count - 1;
        while (leftPoint <= rightPoint)
        {
            int midPoint = leftPoint + ((rightPoint - leftPoint) / 2);


            //Using the implementation of CompareTo, the left and right points can be adjusted to search using a binary search
            if (member.CompareTo(this.members[midPoint]) < 0)
            {
                rightPoint = midPoint - 1;
            }
            else if (member.CompareTo(this.members[midPoint]) > 0)
            {
                leftPoint = midPoint + 1;
            }
            else
            {
                return true;
            }

        }
        return false;
    }


    // Find a given member in this member collection 
    // Pre-condition: nil
    // Post-condition: return the reference of the member object in the member collection, if this member is in the member collection; return null otherwise; member collection remains unchanged
    public IMember Find(IMember member)
    {
        // To be implemented by students in Phase 1
        int leftPoint = 0;
        int rightPoint = count - 1;
        while (leftPoint <= rightPoint)
        {
            int midPoint = leftPoint + ((rightPoint - leftPoint) / 2);


            //Using the implementation of CompareTo, the left and right points can be adjusted to search using a binary search
            if (member.CompareTo(this.members[midPoint]) < 0)
            {
                rightPoint = midPoint - 1;
            }
            else if (member.CompareTo(this.members[midPoint]) > 0)
            {
                leftPoint = midPoint + 1;
            }
            else
            {
                return this.members[midPoint];
            }

        }
        return null;
    }

    // Remove all the members in this member collection
    // Pre-condition: nil
    // Post-condition: no member in this member collection 
    public void Clear()
    {
        for (int i = 0; i < count; i++)
        {
            this.members[i] = null;
        }
        count = 0;
    }

    // Return a string containing the information about all the members in this member collection.
    // The information includes last name, first name and contact number in this order
    // Pre-condition: nil
    // Post-condition: a string containing the information about all the members in this member collection is returned
    public string ToString()
    {
        string s = "";
        for (int i = 0; i < count; i++)
            s = s + members[i].ToString() + "\n";
        return s;
    }


}
