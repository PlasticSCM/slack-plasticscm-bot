# PlasticBot
###### A bridge between Slack and PlasticSCM `cm` utility
-------------------------------------------------------
### Available commands
1. `plastic latest` 
2. `plastic list`
3. `plastic switch`  

------------------

## 1. `plastic latest`
A bind to the `cm find [···] --nototal` command to quickly gather useful information about a repository.

### 1.1. `plastic latest branches`
Shows the latest branches created in a repository, along with the creation date, the owner of the branch and the creation comment.
> [`br:/main/developer`] on _abr 13 at 21:37_ by __sergioluis__: Our develop branch.

> [`branch`] on {date} by __{owner}__: {comment}

### 1.2. `plastic latest changesets [br=? | branch=?]`
Shows the latest changesets checked in a repository, along with the checkin date, its owner and its comment.

>[`cs:47@br:/main/developer/task002`] on abr 23 at 17:02 by __sergio__: Changed padding between list elements in the Ticket list.

If yoy specify a branch using the `br=my/branch/name` or `branch=my/branch/name` modifiers, only the latest changesets on that branch will be shown.

>[`cs:16@br:/main`] _on abr 12 at 19:07_ by __sergio__: New private files.  
>[`cs:17@br:/main`] _on abr 12 at 19:08_ by __sergio__: Changed cloaked/hidden files.  
>[`cs:18@br:/main`] _on abr 12 at 19:17_ by __sergio__: .idea directory fixes  
>[`cs:20@br:/main`] _on abr 12 at 19:30_ by __sergio__: Merge fix back into main. 

>[`changeset@branch`] on _{date}_ by __{owner}__: {comment}

### 1.3. `plastic latest merges {srcbranch=? | dstbranch=?}`

Shows the latest merges in the specified branches. For example, if you set `dstbranch` to `main` the latests merges to `main` will be shown.

>Latest merges found with destination `main@myRepo`:  
>[`br:/main/fix001@5` __--->__ `br:/main@6`] merge _on abr 12 at 11:59_ by __sergio__  
>[`br:/main/refactor001@14` __--->__ `br:/main@15`] merge _on abr 12 at 19:07_ by __sergio__  
>[`br:/main/fix002@19` __--->__ `br:/main@20`] merge _on abr 12 at 19:30_ by __sergio__  
>[`br:/main/developer@40` __--->__ `br:/main@41`] merge _on abr 23 at 13:11_ by __sergio__  
>[`br:/main/developer@48` __--->__ `br:/main@49`] merge _on abr 23 at 17:05_ by __sergio__  

>[`srcbranch@changeset` __--->__ `dstbranch@changeset`] {merge/cherrypick} on {date} by __{owner}__

## 2. `plastic list`
A bind to the `cm find` and `cm lrep` commands.

### 2.1. `plastic list [rep | repositories]`
Lists available repositories in the server.

> `plastic list rep`  

> Repositories found:  
> `#2 - Talky`  
> `#4 - osTicket`  
> `#6 - PlasticBot`

> `#{N} - {Repo name}`

### 2.2. `plastic list [lb | labels]`
Lists the latest labels created in the repo

>`plastic list lb`

>Labels found on `osTicket`:  
>[`version0.1`] on _abr 12 at 19:32_ by __sergio__  
>[`version0.2`] on _abr 23 at 13:14_ by __sergio__  
>[`version0.3`] on _abr 23 at 17:06_ by __sergio__

>[`name`] on _{date}_ by __{owner}__

## 3. `plastic switch`
Currently it only supports one kind of switch. It is not a bind to a `cm` command.

### 3.1. `plastic switch rep to=?`
It will check if you're trying to switch to a non-existent repository. If everything is good, next commands will run against the selected repository.

>`plastic switch rep to=blablabla`  
>could you repeat? I couldn't find `blablabla`  

>`plastic switch rep to=osTicket`  
>Switched to repository `osTicket`
