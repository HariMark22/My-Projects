// Phase 2
// An implementation of MovieCollection ADT
// 2022


using System;

//A class that models a node of a binary search tree
//An instance of this class is a node in a binary search tree 
public class BTreeNode
{
    private IMovie movie; // movie
    private BTreeNode lchild; // reference to its left child 
    private BTreeNode rchild; // reference to its right child

    public BTreeNode(IMovie movie)
    {
        this.movie = movie;
        lchild = null;
        rchild = null;
    }

    public IMovie Movie
    {
        get { return movie; }
        set { movie = value; }
    }

    public BTreeNode LChild
    {
        get { return lchild; }
        set { lchild = value; }
    }

    public BTreeNode RChild
    {
        get { return rchild; }
        set { rchild = value; }
    }
}

// invariant: no duplicates in this movie collection
public class MovieCollection : IMovieCollection
{
    private BTreeNode root; // movies are stored in a binary search tree and the root of the binary search tree is 'root' 
    private int count; // the number of (different) movies currently stored in this movie collection 


    // get the number of movies in this movie colllection 
    // pre-condition: nil
    // post-condition: return the number of movies in this movie collection and this movie collection remains unchanged
    public int Number { get { return count; } }

    // constructor - create an object of MovieCollection object
    public MovieCollection()
    {
        root = null;
        count = 0;
    }

    // Check if this movie collection is empty
    // Pre-condition: nil
    // Post-condition: return true if this movie collection is empty; otherwise, return false.
    public bool IsEmpty()
    {
        //To be completed
        return count < 1 && root == null;
    }

    // Insert a movie into this movie collection
    // Pre-condition: nil
    // Post-condition: the movie has been added into this movie collection and return true,
    //		if the movie is not in this movie collection; otherwise, the movie has not been
    //		added into this movie collection and return false.
    public bool Insert(IMovie movie)
    {
        //if collection is empty, add movie
        if (root == null)
        {
            root = new BTreeNode(movie);
            count++;
            return true;
        }
        else //insert
            return Insert(movie, root);

    }

    //private method used to insert movie into collection
    private bool Insert(IMovie movie, BTreeNode ptr)
    {
        //if left child
        if (movie.CompareTo(ptr.Movie) < 0)
        {
            if (ptr.LChild == null)
            {
                ptr.LChild = new BTreeNode(movie);
                count++;
                return true;
            }
            else //look for next available child spot
                return Insert(movie, ptr.LChild);
        }
        //elif right child
        else if (movie.CompareTo(ptr.Movie) > 0)
        {
            if (ptr.RChild == null)
            {
                ptr.RChild = new BTreeNode(movie);
                count++;
                return true;
            }
            else //look for next available child spot
                return Insert(movie, ptr.RChild);

        }
        else return false;
    }


    // Delete a movie from this movie collection
    // Pre-condition: nil
    // Post-condition: the movie is removed out of this movie collection and return true, if it is in this movie collection; return false, if it is not in this movie collection
    public bool Delete(IMovie movie)
    {
        //To be completed
        // search for item and its parent
        BTreeNode ptr = root; // search reference
        BTreeNode parent = null; // parent of ptr
        while ((ptr != null) && (movie.CompareTo(ptr.Movie) != 0))
        {
            parent = ptr;
            if (movie.CompareTo(ptr.Movie) < 0) // move to the left child of ptr
                ptr = ptr.LChild;
            else
                ptr = ptr.RChild;
        }

        if (ptr != null) // if the search was successful
        {
            // case 3: item has two children
            if ((ptr.LChild != null) && (ptr.RChild != null))
            {
                // find the right-most node in left subtree of ptr
                if (ptr.LChild.RChild == null) // a special case: the right subtree of ptr.LChild is empty
                {
                    ptr.Movie = ptr.LChild.Movie;
                    ptr.LChild = ptr.LChild.LChild;
                }
                else
                {
                    BTreeNode p = ptr.LChild;
                    BTreeNode pp = ptr; // parent of p
                    while (p.RChild != null)
                    {
                        pp = p;
                        p = p.RChild;
                    }
                    // copy the item at p to ptr
                    ptr.Movie = p.Movie;
                    pp.RChild = p.LChild;
                }
            }
            else // cases 1 & 2: item has no or only one child
            {
                BTreeNode c;
                if (ptr.LChild != null)
                    c = ptr.LChild;
                else
                    c = ptr.RChild;

                // remove node ptr
                if (ptr == root) //need to change root
                    root = c;
                else
                {
                    if (ptr == parent.LChild)
                        parent.LChild = c;
                    else
                        parent.RChild = c;
                }
            }
            count--;
            return true;
        }
        return false;
    }

    // Search for a movie in this movie collection
    // pre: nil
    // post: return true if the movie is in this movie collection;
    //	     otherwise, return false.
    public bool Search(IMovie movie)
    {
        //To be completed
        return Search(movie, root);

    }
    private bool Search(IMovie movie, BTreeNode r)
    {
        if (r != null)
        {
            if (movie.CompareTo(r.Movie) == 0)
                return true;
            else
                if (movie.CompareTo(r.Movie) < 0)
                return Search(movie, r.LChild);
            else
                return Search(movie, r.RChild);
        }
        else
            return false;
    }

    // Search for a movie by its title in this movie collection  
    // pre: nil
    // post: return the reference of the movie object if the movie is in this movie collection;
    //	     otherwise, return null.
    public IMovie Search(string movietitle)
    {
        //To be completed
        return Search(movietitle, root);
    }

    private IMovie Search(string movietitle, BTreeNode r)
    {
        if (r != null)
        {
            if (movietitle.CompareTo(r.Movie.Title) == 0)
                return r.Movie;
            else
                if (movietitle.CompareTo(r.Movie.Title) < 0)
                return Search(movietitle, r.LChild);
            else
                return Search(movietitle, r.RChild);
        }
        else
            return null;
    }



    // Store all the movies in this movie collection in an array in the dictionary order by their titles
    // Pre-condition: nil
    // Post-condition: return an array of movies that are stored in dictionary order by their titles
    public IMovie[] ToArray()
    {
        IMovie[] array = new IMovie[count];
        int i = 0;
        ToArray(root, ref array, ref i);
        if (array == null) array = new IMovie[0];
        return array;
    }

    private void ToArray(BTreeNode root, ref IMovie[] movieArray, ref int i)
    {
        if (root != null)
        {
            ToArray(root.LChild, ref movieArray, ref i);
            if (root.Movie != null)
            {
                movieArray[i] = root.Movie;
                i++;
            }
            ToArray(root.RChild, ref movieArray, ref i);
        }
    }



    // Clear this movie collection
    // Pre-condotion: nil
    // Post-condition: all the movies have been removed from this movie collection 
    public void Clear()
    {
        //To be completed
        root = null;
        count = 0;
    }




}





